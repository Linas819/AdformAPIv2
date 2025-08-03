namespace AdformAPI.Models
{
    public class NewOrder
    {
        public string OrderName { get; set; }
        public int[] ProductIds { get; set; }
        public int[] ProductQuantities { get; set; }
    }
}
