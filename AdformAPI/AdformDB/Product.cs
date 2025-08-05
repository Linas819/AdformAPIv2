using System;
using System.Collections.Generic;

namespace AdformAPI.AdformDB;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public double ProductPrice { get; set; }

    public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();

    public virtual ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
}
