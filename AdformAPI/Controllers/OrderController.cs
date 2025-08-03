using AdformAPI.Models;
using AdformAPI.Services;
using Azure;
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
        public IActionResult GetOrderInvoice(int orderId)
        {
            OrderInvoice orderInvoice = orderService.GetOrderInvoice(orderId);
            return (Ok(new
            {
                OrderInvoice = orderInvoice
            }));
        }
    }
}
