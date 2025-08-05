namespace AdformAPI.Models
{
    public class OrderLineDetail
    {
        public string OrderName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public int ProductQuantity { get; set; }
        public int DiscountPercentage { get; set; }
        public int DiscountMinimalQuantity { get; set; }
    }
}
