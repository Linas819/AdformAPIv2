using AdformAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdformAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private ProductService productService;
        public ProductController(ProductService productService)
        {
            this.productService = productService;
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
