namespace BeautySalon.Domain.Entities;

public class Master : Person
{
    public required string PassportNumber { get; set; }
    public required string Specialization { get; set; }
    public required int ExperienceYears { get; set; }
}
