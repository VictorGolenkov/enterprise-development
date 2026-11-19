using BeautySalon.Domain.Enums;

namespace BeautySalon.Domain.Entities;

public abstract class Person
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string LastName { get; set; }
    public required string FirstName { get; set; }
    public string? Patronymic { get; set; }
    public required Gender Gender { get; set; }

    public string FullName => string.Join(" ",
        new[] { LastName, FirstName, Patronymic }
        .Where(s => !string.IsNullOrWhiteSpace(s)));
}
