using System;
using System.Collections.Generic;

namespace DemoForRepeat.Models;

public partial class PickupPoint
{
    public int PointId { get; set; }

    public string PointName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? Phone { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
