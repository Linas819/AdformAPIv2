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
