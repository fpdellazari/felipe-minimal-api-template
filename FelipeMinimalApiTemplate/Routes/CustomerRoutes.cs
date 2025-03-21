using FelipeMinimalApiTemplate.Data;
using FelipeMinimalApiTemplate.Models.DTOs;
using FelipeMinimalApiTemplate.Models.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FelipeMinimalApiTemplate.Routes;

public static class CustomerRoutes
{
    public static void MapCustomerRoutes(this WebApplication app)
    {
        var route = app.MapGroup("customer");

        route.MapGet("", async (ApplicationDbContext context) =>
        {
            var customers = await context.Customers.AsNoTracking().ToListAsync();

            return Results.Ok(customers);
        });

        route.MapGet("{id:guid}", async (Guid id, ApplicationDbContext context) =>
        {
            var customer = await context.Customers.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);

            if (customer is null) return Results.NotFound("Cliente não encontrado.");

            return customer is null ? Results.NotFound() : Results.Ok(customer);
        });

        route.MapPost("", async (CreateCustomerDTO createCustomerDTO, ApplicationDbContext context, IValidator<CreateCustomerDTO> validator) =>
        {
            var validationResult = await validator.ValidateAsync(createCustomerDTO);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var customer = new Customer()
            {
                Name = createCustomerDTO.Name,
                Age = createCustomerDTO.Age,
                Email = createCustomerDTO.Email
            };

            await context.Customers.AddAsync(customer);
            await context.SaveChangesAsync();

            return Results.Ok(customer);
        });

        route.MapPut("{id:guid}", async (Guid id, UpdateCustomerDTO updateCustomerDTO, ApplicationDbContext context, IValidator<UpdateCustomerDTO> validator) =>
        {
            var validationResult = await validator.ValidateAsync(updateCustomerDTO);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var customer = await context.Customers.FindAsync(id);

            if (customer is null) return Results.NotFound("Cliente não encontrado.");

            customer.Name = updateCustomerDTO.Name;
            customer.Age = updateCustomerDTO.Age;
            customer.Email = updateCustomerDTO.Email;
            await context.SaveChangesAsync();

            return Results.Ok(customer);
        });

        route.MapPatch("{id:guid}", async (Guid id, PartialUpdateCustomerDTO partialUpdateCustomerDTO, ApplicationDbContext context, IValidator<PartialUpdateCustomerDTO> validator) =>
        {
            var validationResult = await validator.ValidateAsync(partialUpdateCustomerDTO);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var customer = await context.Customers.FindAsync(id);

            if (customer is null) return Results.NotFound("Cliente não encontrado.");

            if (partialUpdateCustomerDTO.Name is not null) customer.Name = partialUpdateCustomerDTO.Name;
            if (partialUpdateCustomerDTO.Email is not null) customer.Email = partialUpdateCustomerDTO.Email;
            if (partialUpdateCustomerDTO.Age is not null) customer.Age = (int)partialUpdateCustomerDTO.Age;

            await context.SaveChangesAsync();

            return Results.Ok(customer);
        });

        route.MapDelete("{id:guid}", async (ApplicationDbContext context, Guid id) =>
        {
            var customer = await context.Customers.FindAsync(id);

            if (customer is null) return Results.NotFound("Cliente não encontrado.");

            context.Customers.Remove(customer);
            await context.SaveChangesAsync();

            return Results.Ok(customer);
        });
    }
}

