using Domain.Common;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace WebAPI.Middlewares
{
    public sealed class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly IProblemDetailsService _problemDetails;
        private readonly IHostEnvironment _env;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(
            IProblemDetailsService problemDetails,
            IHostEnvironment env,
            ILogger<ErrorHandlingMiddleware> logger)
        {
            _problemDetails = problemDetails;
            _env = env;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var (status, title, errors) = Map(ex);

                // Loga com severidade adequada
                if ((int)status >= 500)
                    _logger.LogError(ex, "Unhandled exception");
                else
                    _logger.LogWarning(ex, "Handled business/validation exception");

                context.Response.StatusCode = (int)status;

                var pd = new ProblemDetails
                {
                    Status = (int)status,
                    Title = title,
                    Type = $"https://httpstatuses.com/{(int)status}",
                    Detail = _env.IsDevelopment() ? ex.Message : null,
                    Instance = context.TraceIdentifier
                };

                if (errors is not null)
                {
                    var vpd = new HttpValidationProblemDetails(errors)
                    {
                        Status = pd.Status,
                        Title = pd.Title,
                        Type = pd.Type,
                        Detail = pd.Detail,
                        Instance = pd.Instance
                    };

                    await _problemDetails.WriteAsync(new ProblemDetailsContext
                    {
                        HttpContext = context,
                        ProblemDetails = vpd
                    });
                    return;
                }

                await _problemDetails.WriteAsync(new ProblemDetailsContext
                {
                    HttpContext = context,
                    ProblemDetails = pd
                });
            }
        }

        private static (HttpStatusCode status, string title, IDictionary<string, string[]>? errors) Map(Exception ex)
        {
            return ex switch
            {
                ValidationException fv => (HttpStatusCode.BadRequest, "Validation failed",
                    fv.Errors
                      .GroupBy(e => e.PropertyName)
                      .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())),

                // Recurso não encontrado
                NotFoundException => (HttpStatusCode.NotFound, "Resource not found", null),

                // Conflitos de concorrência/único
                DbUpdateConcurrencyException => (HttpStatusCode.Conflict, "Concurrency conflict", null),
                DbUpdateException => (HttpStatusCode.Conflict, "Data update conflict", null),

                // Regras quebradas do domínio → 422 (ou 409 se preferir)
                DomainRuleException => (HttpStatusCode.UnprocessableEntity, "Domain rule violated", null),

                // Inputs ruins fora do validator (guard clauses)
                ArgumentException or ArgumentOutOfRangeException
                    => (HttpStatusCode.BadRequest, "Invalid argument", null),

                // Autorização/autenticação
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized", null),

                // Genérico → 500
                _ => (HttpStatusCode.InternalServerError, "Unexpected error", null)
            };
        }
    }
}
