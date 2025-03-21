namespace FelipeMinimalApiTemplate.Models.Entities;

public class Customer
{
    public Customer()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; init; }
    public required string Name { get; set; }
    public int Age { get; set; }
    public string? Email { get; set; }
}

