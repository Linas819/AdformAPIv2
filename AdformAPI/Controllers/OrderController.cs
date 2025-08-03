using AdformAPI.Models;
using AdformAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdformAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private OrderService orderService;
        public OrderController(OrderService orderService)
        {
            this.orderService = orderService;
        }
        [HttpGet]
        public IActionResult GetOrders(int page = 0, int pageSize = 0, int productPage = 0, int productPageSize = 0) {
            List<OrderDetail> orders = new List<OrderDetail>();
            orders = orderService.GetOrders(page, pageSize, productPage, productPageSize);
            return (Ok(new
            {
                Orders = orders,
            }));
        }
        [HttpGet]
        [Route("invoice")]
        public IActionResult GetOrderInvoice(int orderId)
        {
            OrderInvoice orderInvoice = orderService.GetOrderInvoice(orderId);
            return (Ok(new
            {
                OrderInvoice = orderInvoice
            }));
        }
        [HttpPost]
        public IActionResult CreateOrder(NewOrder newOrder)
        {
            DatabaseSaveChangesResponse response = orderService.CreateOrder(newOrder);
            return (Ok(new
            {
                Message = response.Message
            }));
        }
    }
}
