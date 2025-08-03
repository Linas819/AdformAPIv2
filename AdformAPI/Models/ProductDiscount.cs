namespace AdformAPI.Models
{
    public class ProductDiscount
    {
        public string ProductName { get; set; }
        public int DiscountPercentage { get; set; }
        public int OrderCount { get; set; }
        public double TotalAmount { get; set; }
    }
}
