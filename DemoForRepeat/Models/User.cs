using System;
using System.Collections.Generic;

namespace DemoForRepeat.Models;

public partial class User
{
    public int UserId { get; set; }

    public int RoleId { get; set; }

    public string Email { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? Phone { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual UserRole Role { get; set; } = null!;
}
