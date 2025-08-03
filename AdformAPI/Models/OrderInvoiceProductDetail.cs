namespace AdformAPI.Models
{
    public class OrderInvoiceProductDetail : ProductDetail
    {
        public int ProductQuantity { get; set; }
        public int DiscountPercentage { get; set; }
        public int DiscountMinimalQuantity { get; set; }
    }
}
