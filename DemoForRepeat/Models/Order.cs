using System;
using System.Collections.Generic;

namespace DemoForRepeat.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? UserId { get; set; }

    public int PickupPointId { get; set; }

    public string? OrderStatus { get; set; }

    public virtual ICollection<OrdersProduct> OrdersProducts { get; set; } = new List<OrdersProduct>();

    public virtual PickupPoint PickupPoint { get; set; } = null!;

    public virtual User? User { get; set; }
}
