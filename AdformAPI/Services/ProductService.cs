using AdformAPI.Exceptions;
using AdformAPI.Models;
using AdformAPI.Repositories;

namespace AdformAPI.Services
{
    public class ProductService
    {
        private ProductRepository productRepository;
        public ProductService(ProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        public List<ProductDetail> GetProducts(string productName, int page, int pageSize)
        {
            if (page < 0 || pageSize < 0)
                throw new ApiException(500, "Page and/or page size cannot be lower than 0");
            int dataLimit = page * pageSize;
            IQueryable<ProductDetail> productDetailsQuery = productRepository.GetProducts(productName);
            List<ProductDetail> products = dataLimit == 0
                ? productDetailsQuery.ToList()
                : productDetailsQuery
                    .Skip(dataLimit - pageSize)
                    .Take(pageSize)
                    .ToList();
            if (products.Count() == 0)
                throw new ApiException(404, "No products found");
            return products;

        }
        public ProductDiscount GetProductDiscount(int discountId)
        {
            List<DiscountProductOrderLine> orderLines = productRepository.GetDiscountProductOrderLines(discountId);
            if (orderLines.Count() == 0)
                throw new ApiException(404, "Discount orders with ID: " + discountId + " not found");
            ProductDiscount productDiscount = new ProductDiscount();
            productDiscount.ProductName = orderLines.First().ProductName;
            productDiscount.DiscountPercentage = orderLines.First().DiscountPercentage;
            productDiscount.OrderCount = orderLines.Where(x => x.OrderId != 0).Select(x => x.OrderId).Distinct().Count();
            productDiscount.TotalAmount = orderLines.Sum(x => x.ProductQuantity * (x.ProductPrice -
                (x.ProductPrice * ((double)x.DiscountPercentage / 100))));
            return productDiscount;
        }
        public DatabaseSaveChangesResponse CreateProduct(NewProduct newProduct)
        {
            DatabaseSaveChangesResponse responce = new DatabaseSaveChangesResponse();
            productRepository.CreateProduct(newProduct);
            responce = productRepository.SaveAdformDatabaseChange();
            return responce;
        }
        public DatabaseSaveChangesResponse CreateProductDiscount(NewProductDiscount newProductDiscount)
        {
            DatabaseSaveChangesResponse response = new DatabaseSaveChangesResponse();
            productRepository.CreateProductDiscount(newProductDiscount);
            response = productRepository.SaveAdformDatabaseChange();
            return response;
        }
    }
}
