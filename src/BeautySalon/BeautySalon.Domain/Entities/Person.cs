using BeautySalon.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeautySalon.Domain.Entities;

public abstract class Person
{
    public Guid Id { get; set; }= Guid.NewGuid();
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? Patronymic { get; set; } = string.Empty;
    public Gender Gender { get; set; }

    public string FullName => string.Join(" ",
        new[] { LastName, FirstName, Patronymic }
        .Where(s => !string.IsNullOrWhiteSpace(s)));
}
