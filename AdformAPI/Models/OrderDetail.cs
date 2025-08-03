namespace AdformAPI.Models
{
    public class OrderDetail
    {
        public int OrderId { get; set; }
        public string OrderName { get; set; }
        public List<OrderProductDetail> OrderProducts { get; set; } = new List<OrderProductDetail>();
    }
}
