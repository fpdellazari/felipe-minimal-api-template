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

        route.MapPost("", async (CreateUpdateCustomerDTO createUpdateCustomer, ApplicationDbContext context, IValidator<CreateUpdateCustomerDTO> validator) =>
        {
            var validationResult = await validator.ValidateAsync(createUpdateCustomer);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var customer = new Customer()
            {
                Name = createUpdateCustomer.Name,
                Age = createUpdateCustomer.Age,
                Email = createUpdateCustomer.Email
            };

            await context.Customers.AddAsync(customer);
            await context.SaveChangesAsync();

            return Results.Ok(customer);
        });

        route.MapPut("{id:guid}", async (Guid id, CreateUpdateCustomerDTO createUpdateCustomer, ApplicationDbContext context, IValidator<CreateUpdateCustomerDTO> validator) =>
        {
            var validationResult = await validator.ValidateAsync(createUpdateCustomer);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var customer = await context.Customers.FindAsync(id);

            if (customer is null) return Results.NotFound("Cliente não encontrado.");

            customer.Name = createUpdateCustomer.Name;
            customer.Age = createUpdateCustomer.Age;
            customer.Email = createUpdateCustomer.Email;
            await context.SaveChangesAsync();

            return Results.Ok(customer);
        });

        route.MapPatch("{id:guid}", async (Guid id, PartialUpdateCustomerDTO partialUpdateCustomer, ApplicationDbContext context, IValidator<PartialUpdateCustomerDTO> validator) =>
        {
            var validationResult = await validator.ValidateAsync(partialUpdateCustomer);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var customer = await context.Customers.FindAsync(id);

            if (customer is null) return Results.NotFound("Cliente não encontrado.");

            if (partialUpdateCustomer.Name is not null) customer.Name = partialUpdateCustomer.Name;
            if (partialUpdateCustomer.Email is not null) customer.Email = partialUpdateCustomer.Email;
            if (partialUpdateCustomer.Age is not null) customer.Age = (int)partialUpdateCustomer.Age;

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

