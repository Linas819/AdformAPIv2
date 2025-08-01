using System;
using System.Collections.Generic;

namespace AdformAPI.AdformDB;

public partial class Order
{
    public int OrderId { get; set; }

    public string OrderName { get; set; } = null!;

    public virtual ICollection<Orderline> Orderlines { get; set; } = new List<Orderline>();
}
