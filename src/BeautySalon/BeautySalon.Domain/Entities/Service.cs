using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace BeautySalon.Domain.Entities;

public class Service
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Category {  get; set; } = string.Empty;
    public decimal Price  { get; set; }
    public TimeSpan Duration { get; set; }
}
