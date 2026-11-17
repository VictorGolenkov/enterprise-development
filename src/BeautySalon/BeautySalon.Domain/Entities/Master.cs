namespace BeautySalon.Domain.Entities;

public class Master: Person
{
    public string PassportNumber { get; set; } = string.Empty;
    public string Specialization {  get; set; } = string.Empty;
    public int ExperienceYears { get; set; }
}
