namespace AdformAPI.Models
{
    public class DiscountProductOrderLine
    {
        public string ProductName { get; set; }
        public int? DiscountPercentage { get; set; }
        public int OrderId { get; set; }
        public int? ProductQuantity { get; set; }
        public double? ProductPrice { get; set; }
    }
}
