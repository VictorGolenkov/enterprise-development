namespace BeautySalon.Domain.Entities;

public class Client : Person
{
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
}
