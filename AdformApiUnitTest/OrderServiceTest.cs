using AdformAPI.AdformDB;
using AdformAPI.Exceptions;
using AdformAPI.Models;
using AdformAPI.Repositories;
using AdformAPI.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AdformApiUnitTest
{
    public class OrderServiceTest
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
        public OrderService OrderService()
        {
            Mock<AdformDatabaseContext> mockContext = MockContext();
            OrderRepository orderRepository = new OrderRepository(mockContext.Object);
            OrderService orderService = new OrderService(orderRepository);
            return orderService;
        }
        [Fact]
        public void GetOrderName()
        {
            int page = 1;
            int pageSize = 1;
            int productPage = 1;
            int productPageSize = 1;
            List<OrderDetail> orders = OrderService().GetOrders(page, pageSize, productPage, productPageSize);
            Assert.Equal(orders[0].OrderName, Orders[0].OrderName);
        }
        [Fact]
        public void GetOrderId()
        {
            int page = 1;
            int pageSize = 1;
            int productPage = 1;
            int productPageSize = 1;
            List<OrderDetail> orders = OrderService().GetOrders(page, pageSize, productPage, productPageSize);
            Assert.Equal(orders.First().OrderId, Orders.First().OrderId);
        }
        [Fact]
        public void GetOrder_ReturnOrderCount()
        {
            int page = 1;
            int pageSize = 2;
            int productPage = 1;
            int productPageSize = 1;
            List<OrderDetail> orders = OrderService().GetOrders(page, pageSize, productPage, productPageSize);
            Assert.Equal(orders.Count(), pageSize);
        }
        [Fact]
        public void GetOrderProduct_ReturnProductCount()
        {
            int page = 1;
            int pageSize = 1;
            int productPage = 1;
            int productPageSize = 2;
            List<OrderDetail> orders = OrderService().GetOrders(page, pageSize, productPage, productPageSize);
            Assert.Equal(orders.First().OrderProducts.Count(), productPageSize);
        }
        [Fact]
        public void GetOrderProduct_ThrowApiException_IncorrectPaginationSet()
        {
            int page = 1;
            int pageSize = 1;
            int productPage = 1;
            int productPageSize = -2;
            ApiException ex = Assert.Throws<ApiException>(() => { OrderService().GetOrders(page, pageSize, productPage, productPageSize); });
            Assert.Equal(400, ex.StatusCode);
            Assert.Equal("(Product) Page size cannot be lower than 0", ex.Message);
        }
        [Fact]
        public void GetOrderInvoice_ReturnOrderId()
        {
            int orderId = 1;
            OrderInvoice order = OrderService().GetOrderInvoice(orderId);
            Assert.Equal(order.OrderId, Orders.First().OrderId);
        }
        [Fact]
        public void GetOrderInvoice_ReturnOrderName()
        {
            int orderId = 1;
            OrderInvoice order = OrderService().GetOrderInvoice(orderId);
            Assert.Equal(order.OrderName, Orders.First().OrderName);
        }
        [Fact]
        public void GetOrderInvoice_ReturnProductName()
        {
            int orderId = 1;
            OrderInvoice order = OrderService().GetOrderInvoice(orderId);
            Assert.Equal(order.Products.First().ProductName, Products.First().ProductName);
        }
        [Fact]
        public void GetOrderInvoice_ProductQuantity()
        {
            int orderId = 1;
            OrderInvoice order = OrderService().GetOrderInvoice(orderId);
            Assert.Equal(order.Products.First().ProductQuantity, OrderLines.First().ProductQuantity);
        }
        [Fact]
        public void GetOrderInvoice_ThrowApiException_NoOrderFound()
        {
            int orderId = 0;
            ApiException ex = Assert.Throws<ApiException>(() => { OrderService().GetOrderInvoice(orderId); });
            Assert.Equal(404, ex.StatusCode);
            Assert.Equal("Order Id: " + orderId + " has no products", ex.Message);
        }
        [Fact]
        public void CreateOrder_ReturnResponseCode()
        {
            int responseCode = 200;
            NewOrder newOrder = new NewOrder()
            {
                ProductIds = [3],
                ProductQuantities = [20]
            };
            DatabaseSaveChangesResponse response = OrderService().CreateOrder(newOrder);
            Assert.Equal(responseCode, response.StatusCode);
        }
        [Fact]
        public void CreateOrder_ReturnResponseMessage()
        {
            string message = "Data posted";
            NewOrder newOrder = new NewOrder()
            {
                ProductIds = [3],
                ProductQuantities = [20]
            };
            DatabaseSaveChangesResponse response = OrderService().CreateOrder(newOrder);
            Assert.Equal(message, response.Message);
        }
    }
}
