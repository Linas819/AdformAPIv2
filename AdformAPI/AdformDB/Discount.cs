using System;
using System.Collections.Generic;

namespace AdformAPI.AdformDB;

public partial class Discount
{
    public int DiscountId { get; set; }

    public int ProductId { get; set; }

    public int? DiscountPercentage { get; set; }

    public int? MinimalQuantity { get; set; }

    public virtual Product Product { get; set; } = null!;
}
