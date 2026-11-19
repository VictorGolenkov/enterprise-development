namespace BeautySalon.Domain.Entities;

public class Client : Person
{
    public required string PhoneNumber { get; set; }
    public required DateTime BirthDate { get; set; }
}
