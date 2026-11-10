using System;
using System.Collections.Generic;
using System.Text;

namespace BeautySalon.Domain.Entities;

public class Client : Person
{
    public string PhoneNumber { get; set; } = string.Empty;
}
