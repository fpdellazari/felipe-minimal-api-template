using FelipeMinimalApiTemplate.Data;
using FelipeMinimalApiTemplate.Models.DTOs;
using FelipeMinimalApiTemplate.Routes;
using FelipeMinimalApiTemplate.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Banco de dados
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
    options.UseSqlite(connectionString);
});

// Validadores com Fluent Validation
builder.Services.AddScoped<IValidator<CreateUpdateCustomerDTO>, CreateUpdateCustomerValidator>();
builder.Services.AddScoped<IValidator<PartialUpdateCustomerDTO>, PartialUpdateCustomerValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Minhas rotas
app.MapCustomerRoutes();

app.Run();
