using System;
using System.Collections.Generic;

namespace AdformAPI.AdformDB;

public partial class OrderLine
{
    public int OrderLineId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int ProductQuantity { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
