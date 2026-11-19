namespace BeautySalon.Domain.Entities;

public class Appointment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required Client Client { get; set; } = null!;
    public required Master Master { get; set; } = null!;
    public required Service Service { get; set; } = null!;
    public required DateTime AppointmentDate { get; set; }
    public required bool IsRegularClient { get; set; }
    public DateTime EndAppointmentDate => AppointmentDate + Service.Duration;
}
