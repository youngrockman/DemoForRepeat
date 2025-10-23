using System;
using System.Collections.Generic;
using Avalonia.Media.Imaging;

namespace DemoForRepeat.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string? Image { get; set; }

    public Bitmap ParseImage
    {
        get
        {
            return new Bitmap(Image);
        }
    }

    public int? ManufacturerId { get; set; }

    public virtual Manufacturer? Manufacturer { get; set; }

    public virtual ICollection<OrdersProduct> OrdersProducts { get; set; } = new List<OrdersProduct>();
}
