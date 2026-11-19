namespace BeautySalon.Domain.Entities;

public class Service
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required string Category { get; set; }
    public required decimal Price { get; set; }
    public required TimeSpan Duration { get; set; }
}
