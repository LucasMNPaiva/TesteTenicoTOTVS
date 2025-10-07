using Application.Implementations.Products.Commands;
using Application.Implementations.Products.Services;
using Domain.Models;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using WebAPI.Validation;

namespace WebAPI.EndpointMapping
{
    public static class ProductEndpoint
    {
        public static void MapProductEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/products").WithTags(nameof(Product));

            group.MapGet("/", async (IProductAppService svc) => Results.Ok(await svc.ListAsync()));
            group.MapGet("/{id:Guid}", async (Guid id, IProductAppService svc)
                => (await svc.GetAsync(id)) is { } dto ? Results.Ok(dto) : Results.NotFound());

            group.MapPost("/", async (CreateProductCommand cmd,IProductAppService svc) =>
            {
                var dto = await svc.CreateAsync(cmd);
                return Results.Created($"/api/products/{dto.Id}", dto);
            }).AddEndpointFilter<ValidationFilter<CreateProductCommand>>();

            group.MapPut("/{id:Guid}", async (Guid id, UpdateProductCommand cmd, IProductAppService svc)
                => await svc.UpdateAsync(id, cmd) ? Results.NoContent() : Results.NotFound()).AddEndpointFilter<ValidationFilter<UpdateProductCommand>>();

            group.MapDelete("/{id:Guid}", async (Guid id, IProductAppService svc)
                => await svc.DeleteAsync(id) ? Results.NoContent() : Results.NotFound());

        }
    }
}
