namespace AdformAPI.Models
{
    public class OrderInvoice
    {
        public int OrderId { get; set; }
        public string OrderName { get; set; }
        public double TotalPrice { get; set; }
        public List<OrderInvoiceProductDetail> Products { get; set; } = new List<OrderInvoiceProductDetail>();
    }
}
