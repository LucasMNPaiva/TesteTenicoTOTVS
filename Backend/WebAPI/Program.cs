using Application.Implementations.Products.Services;
using Application.Implementations.Products.Validators;
using Domain.Interfaces;
using FluentValidation;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using WebAPI.EndpointMapping;
using WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Contexts
builder.Services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("products-db"));
//Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddGlobalErrorHandling();
//Services
builder.Services.AddScoped<IProductAppService, ProductAppService>();
//CORS
builder.Services.AddCors(opt =>
    opt.AddDefaultPolicy(p => p.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200")));
//Validators
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductCommandValidator>();

var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//Middlewares 
app.UseGlobalErrorHandling();
//Endpoints
app.MapProductEndpoints();


app.UseHttpsRedirection();

app.Run();