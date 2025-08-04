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
        /// <summary>
        /// Gets product. Can be specified by product name. Can be paginated if it is set
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     
        ///     GET /product
        /// 
        /// Sample request with product name:
        /// 
        ///     GET /product
        ///     {
        ///         "productName": "Apple"
        ///     }
        ///     
        /// Sample request with pagination:
        /// 
        ///     GET /product
        ///     {
        ///         "page": 1,
        ///         "pageSize": 1
        ///     }
        /// </remarks>
        /// <param name="productName"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <response code="404">Product not found</response>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetProducts(string productName = "", int page = 0, int pageSize = 0)
        {
            List<ProductDetail> products = productService.GetProducts(productName, page, pageSize);
            return (Ok(new
            {
                Products = products
            }));
        }
        /// <summary>
        /// Gets information about a discounted product
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     
        ///     GET /product/discount
        ///     {
        ///         "discountId": 1
        ///     }
        /// </remarks>
        /// <param name="discountId"></param>
        /// <response code="404">Discounted product not found</response>
        /// <returns></returns>
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
        /// <summary>
        /// Create a new product
        /// </summary>
        /// <remarks>
        /// Sample requst:
        ///     
        ///     POST /product
        ///     {
        ///         "productName": "Product"
        ///         "productPrice": 2.1
        ///     }
        /// </remarks>
        /// <param name="newProduct"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateProduct(NewProduct newProduct)
        {
            DatabaseSaveChangesResponse responce = productService.CreateProduct(newProduct);
            return (Ok(new
            {
                Message = responce.Message
            }));
        }
        /// <summary>
        /// Create a discount for a prorduct
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     
        ///     POST /product/discount
        ///     {
        ///         "productId": 1
        ///         "discountPercentage": 10
        ///         "MinimalQuantity": 10
        ///     }
        /// </remarks>
        /// <param name="newProductDiscount"></param>
        /// <response code="803">Product ID does not exist. Percentage/Minimal quantity cannot be 0</response>
        /// <returns></returns>
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
