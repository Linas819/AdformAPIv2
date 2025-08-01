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
        public IActionResult Index()
        {
            return (Ok(new
            {
                Success = true
            }));
        }
    }
}
