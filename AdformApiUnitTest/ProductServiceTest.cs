using AdformAPI.AdformDB;
using AdformAPI.Exceptions;
using AdformAPI.Models;
using AdformAPI.Repositories;
using AdformAPI.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AdformApiUnitTest
{
    public class ProductServiceTest
    {
        List<Product> Products = new List<Product>
        {
            new Product { ProductId = 1, ProductName = "Product 1", ProductPrice = 1 },
            new Product { ProductId = 2, ProductName = "Product 2", ProductPrice = 2.1 },
            new Product { ProductId = 3, ProductName = "Product 3", ProductPrice = 3.21 },
        };
        List<Discount> Discounts = new List<Discount>
        {
            new Discount { DiscountId = 1, ProductId = 1, DiscountPercentage = 20, MinimalQuantity = 20 }
        };
        List<Order> Orders = new List<Order>
        {
            new Order { OrderId = 1, OrderName = "Order 1"},
            new Order { OrderId = 2, OrderName = "Order 2"}
        };
        List<OrderLine> OrderLines = new List<OrderLine>
        {
            new OrderLine { OrderLineId = 1, OrderId = 1, ProductId = 1, ProductQuantity = 20 },
            new OrderLine { OrderLineId = 2, OrderId = 1, ProductId = 2, ProductQuantity = 21 },
            new OrderLine { OrderLineId = 3, OrderId = 2, ProductId = 2, ProductQuantity = 22 },
            new OrderLine { OrderLineId = 4, OrderId = 2, ProductId = 3, ProductQuantity = 23 }
        };
        public static Mock<DbSet<T>> CreateMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return mockSet;
        }
        public Mock<AdformDatabaseContext> MockContext()
        {
            Mock<AdformDatabaseContext> mockContext = new Mock<AdformDatabaseContext>();
            Mock<DbSet<Product>> mockProductSet = CreateMockDbSet(Products);
            Mock<DbSet<Discount>> mockDiscountSet = CreateMockDbSet(Discounts);
            Mock<DbSet<Order>> mockOrderSet = CreateMockDbSet(Orders);
            Mock<DbSet<OrderLine>> mockOrderLineSet = CreateMockDbSet(OrderLines);
            mockContext.Setup(c => c.Products).Returns(mockProductSet.Object);
            mockContext.Setup(c => c.Discounts).Returns(mockDiscountSet.Object);
            mockContext.Setup(c => c.Orders).Returns(mockOrderSet.Object);
            mockContext.Setup(c => c.OrderLines).Returns(mockOrderLineSet.Object);
            return mockContext;
        }
        public ProductService ProductService()
        {
            Mock<AdformDatabaseContext> mockContext = MockContext();
            ProductRepository productRepository = new ProductRepository(mockContext.Object);
            ProductService productService = new ProductService(productRepository);
            return productService;
        }
        [Fact]
        public void GetProductId()
        {
            List<ProductDetail> products = ProductService().GetProducts("", 1, 1);
            Assert.Equal(products[0].ProductId, Products[0].ProductId);
        }
        [Fact]
        public void GetProductName()
        {
            List<ProductDetail> products = ProductService().GetProducts("", 1, 1);
            Assert.Equal(products[0].ProductName, Products[0].ProductName);
        }
        [Fact]
        public void GetProductPrice()
        {
            List<ProductDetail> products = ProductService().GetProducts("", 1, 1);
            Assert.Equal(products[0].ProductPrice, Products[0].ProductPrice);
        }
        [Fact]
        public void GetProductPagination()
        {
            int pagesize = 2;
            List<ProductDetail> products = ProductService().GetProducts("", 1, pagesize);
            Assert.Equal(products.Count(), pagesize);
        }
        [Fact]
        public void GetProduct_ThrowApiException_NoProductFound()
        {
            ApiException ex = Assert.Throws<ApiException>(() => { ProductService().GetProducts("NoProduct", 1, 1); });
            Assert.Equal(404, ex.StatusCode);
            Assert.Equal("No products found", ex.Message);
        }
        [Fact]
        public void GetProduct_ThrowApiException_Incorrectpagination()
        {
            ApiException ex = Assert.Throws<ApiException>(() => { ProductService().GetProducts("", -1, -1); });
            Assert.Equal(500, ex.StatusCode);
            Assert.Equal("Page and/or page size cannot be lower than 0", ex.Message);
        }
        [Fact]
        public void GetProductDiscountId()
        {
            int discountId = 1;
            int productItem = discountId - 1;
            ProductDiscount discount = ProductService().GetProductDiscount(discountId);
            Assert.Equal(discount.ProductName, Products[productItem].ProductName);
        }
        [Fact]
        public void GetProductDiscountPercentage()
        {
            int discountId = 1;
            int discountItem = discountId - 1;
            ProductDiscount discount = ProductService().GetProductDiscount(discountId);
            Assert.Equal(discount.DiscountPercentage, Discounts[discountItem].DiscountPercentage);
        }
        [Fact]
        public void GetProductDiscount_ThrowApiException_DiscountIdNotFound()
        {
            int discountId = 0;
            ApiException ex = Assert.Throws<ApiException>(() => { ProductService().GetProductDiscount(discountId); });
            Assert.Equal(404, ex.StatusCode);
            Assert.Equal("Discount orders with ID: " + discountId + " not found", ex.Message);
        }
        [Fact]
        public void CreateProduct_ReturnResponseCode()
        {
            int responseCode = 200;
            NewProduct product = new NewProduct() 
            {
                ProductName = "Prod",
                ProductPrice = 1
            };
            DatabaseSaveChangesResponse response = ProductService().CreateProduct(product);
            Assert.Equal(responseCode, response.StatusCode);
        }
        [Fact]
        public void CreateProduct_ReturnResponseMessage()
        {
            string message = "Data posted";
            NewProduct product = new NewProduct()
            {
                ProductName = "Prod",
                ProductPrice = 1
            };
            DatabaseSaveChangesResponse response = ProductService().CreateProduct(product);
            Assert.Equal(message, response.Message);
        }
        [Fact]
        public void CreateDiscount_ReturnResponseCode()
        {
            int responseCode = 200;
            NewProductDiscount discount = new NewProductDiscount()
            {
                ProductId = 1,
                DiscountPercentage = 10,
                MinimalQuantity = 10
            };
            DatabaseSaveChangesResponse response = ProductService().CreateProductDiscount(discount);
            Assert.Equal(responseCode, response.StatusCode);
        }
        [Fact]
        public void CreateDiscount_ReturnResponseMessage()
        {
            string message = "Data posted";
            NewProductDiscount discount = new NewProductDiscount()
            {
                ProductId = 1,
                DiscountPercentage = 10,
                MinimalQuantity = 10
            };
            DatabaseSaveChangesResponse response = ProductService().CreateProductDiscount(discount);
            Assert.Equal(message, response.Message);
        }
    }
}