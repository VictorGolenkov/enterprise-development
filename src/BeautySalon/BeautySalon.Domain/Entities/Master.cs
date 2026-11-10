using System;
using System.Collections.Generic;
using System.Text;

namespace BeautySalon.Domain.Entities;

public class Master: Person
{
    public string PassportNumder { get; set; } = string.Empty;
    public string Specialization {  get; set; } = string.Empty;
    public int ExperienceYears { get; set; }
}
