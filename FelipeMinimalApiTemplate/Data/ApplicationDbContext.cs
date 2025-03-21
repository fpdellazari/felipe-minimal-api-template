using FelipeMinimalApiTemplate.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FelipeMinimalApiTemplate.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Customer> Customers { get; set; }
}

