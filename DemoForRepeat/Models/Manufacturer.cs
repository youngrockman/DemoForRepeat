using System;
using System.Collections.Generic;

namespace DemoForRepeat.Models;

public partial class Manufacturer
{
    public int ManufacturerId { get; set; }

    public string ManufacturerName { get; set; } = null!;

    public string? ContactInfo { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
