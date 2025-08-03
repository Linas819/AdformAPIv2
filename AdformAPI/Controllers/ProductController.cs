using AdformAPI.AdformDB;
using AdformAPI.Models;
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
        public IActionResult GetProducts(string productName = "", int page = 0, int pageSize = 0)
        {
            List<ProductDetail> products = productService.GetProducts(productName, page, pageSize);
            return (Ok(new
            {
                Products = products
            }));
        }
        [HttpGet]
        [Route("discount")]
        public IActionResult GetDiscount(int discountId)
        {
            ProductDiscount productDiscount = productService.GetProductDiscount(discountId);
            return (Ok(new
            {
                ProductDiscount = productDiscount
            }));
        }
        [HttpPost]
        public IActionResult CreateProduct(NewProduct newProduct)
        {
            DatabaseSaveChangesResponse responce = productService.CreateProduct(newProduct);
            return (Ok(new
            {
                Message = responce.Message
            }));
        }
        [HttpPost]
        [Route("discount")]
        public IActionResult CreateProductDiscount(NewProductDiscount newProductDiscount)
        {
            DatabaseSaveChangesResponse responce = productService.CreateProductDiscount(newProductDiscount);
            return (Ok(new
            {
                Message = responce.Message
            }));
        }
    }
}
