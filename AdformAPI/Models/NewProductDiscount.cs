using AdformAPI.AdformDB;

namespace AdformAPI.Models
{
    public class NewProductDiscount
    {
        public int ProductId { get; set; }
        public int DiscountPercentage { get; set; }
        public int MinimalQuantity { get; set; }
    }
}
