namespace BeautySalon.Domain.Entities;

public class Appointment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Client Client { get; set; } = null!;
    public Master Master { get; set; } = null!;
    public Service Service { get; set; } = null!;
    public DateTime AppointmentDate { get; set; }
    public bool IsRegularClient { get; set; }

}
