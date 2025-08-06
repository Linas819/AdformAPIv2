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
        /// <summary>
        /// Gets all of the orders.Can return order list by pages if they have been set
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /order
        ///     
        /// Sample request with pagination:
        ///     
        ///     GET /order
        ///     {
        ///         "page": 1,
        ///         "pageSize": 10,
        ///         "productPage": 1,
        ///         "productPageSize": 10
        ///     }
        /// </remarks>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="productPage"></param>
        /// <param name="productPageSize"></param>
        /// <response code="404"> No orders have been found. No products in the order.</response>
        /// <response code="400">Incorrect (product)page(size) set</response>
        /// <returns> Order information and a list of products in the order</returns>
        [HttpGet]
        public IActionResult GetOrders(int page = 0, int pageSize = 0, int productPage = 0, int productPageSize = 0)
        {
            List<OrderDetail> orders = new List<OrderDetail>();
            orders = orderService.GetOrders(page, pageSize, productPage, productPageSize);
            return (Ok(new
            {
                Orders = orders,
            }));
        }
        /// <summary>
        /// Gets the invoice data of a specified order
        /// </summary>
        /// <remakrs>
        /// Sample request:
        ///     Get /order/invoice
        ///     {
        ///         "orderId": 1
        ///     }
        /// </remakrs>
        /// <param name="orderId"></param>
        /// <response code="404">Order not found.</response>
        /// <returns>An invoice information for an order</returns>
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
        /// <summary>
        /// Create a new order
        /// </summary>
        /// <remarks>
        /// Saqmple request:
        /// 
        ///     POST /order
        ///     {
        ///         "orderName": "order1",
        ///         "productIds": [1, 2],
        ///         "productQuantities": [10, 20]
        ///     }
        /// </remarks>
        /// <response code="400">Product name required. Product id list length must match Product quantities list</response>
        /// <response code="500">Invalid upload data: ex.: Quantity cannot be lower than 0</response>
        /// <param name="newOrder"></param>
        /// <returns></returns>
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
