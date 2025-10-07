﻿using FluentValidation;
using FluentValidation.Results;

namespace WebAPI.Validation
{
    public class ValidationFilter<T> : IEndpointFilter where T : class
    {
        private readonly IValidator<T> _validator;
        public ValidationFilter(IValidator<T> validator) => _validator = validator;

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext ctx, EndpointFilterDelegate next)
        {
            var arg = ctx.Arguments.OfType<T>().FirstOrDefault();
            if (arg is null) return await next(ctx);

            ValidationResult result = await _validator.ValidateAsync(arg);
            if (!result.IsValid)
            {
                var errors = result.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

                return TypedResults.ValidationProblem(errors);
            }

            return await next(ctx);
        }
    }
}
