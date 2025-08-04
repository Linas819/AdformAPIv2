using AdformAPI.AdformDB;
using AdformAPI.Exceptions;
using AdformAPI.Models;
using AdformAPI.Repositories;

namespace AdformAPI.Services
{
    public class OrderService
    {
        private OrderRepository repository;
        public OrderService(OrderRepository repository)
        {
            this.repository = repository;
        }
        public List<OrderDetail> GetOrders(int page, int pageSize, int productPage, int productPageSize)
        {
            if (page < 0 || pageSize < 0 || productPage < 0 || productPageSize < 0)
                throw new ApiException(400, "(Product) Page size cannot be lower than 0");
            List<OrderDetail> orders = new List<OrderDetail>();
            List<Order> ords = repository.GetOrders(page * pageSize);
            if(ords.Count() == 0)
                throw new ApiException(404, "No orders found");
            foreach (Order ord in ords) 
            {
                OrderDetail order = new OrderDetail();
                List<OrderProductDetail> orderProducts = repository.GetOrderProducts(ord.OrderId, productPage * productPageSize);
                if (orderProducts.Count() == 0)
                    throw new ApiException(404, "No products found from Order Id: " + ord.OrderId.ToString());
                if (productPage != 0 && productPageSize != 0)
                {
                    int orderProductsCount = orderProducts.Count;
                    int totalPages = (int)Math.Ceiling((decimal)orderProductsCount / pageSize);
                    orderProducts = orderProducts
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
                }
                order.OrderId = ord.OrderId;
                order.OrderName = ord.OrderName;
                order.OrderProducts = orderProducts;
                orders.Add(order);
            }
            if (page != 0 && pageSize != 0)
            {
                int orderCount = orders.Count;
                int totalPages = (int)Math.Ceiling((decimal)orderCount / pageSize);
                orders = orders
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            return orders;
        }
        public OrderInvoice GetOrderInvoice(int orderId)
        {
            List<OrderlineDetail> orderLineDetails = repository.GetOrderlines(orderId);
            if (orderLineDetails.Count() == 0)
                throw new ApiException(404, "Order Id: " + orderId + " has no products");
            OrderInvoice orderInvoice = new OrderInvoice();
            orderInvoice.OrderId = orderId;
            orderInvoice.OrderName = orderLineDetails.First().OrderName;
            foreach (OrderlineDetail line in orderLineDetails) {
                orderInvoice.Products.Add(new OrderInvoiceProductDetail { 
                    ProductId = line.ProductId,
                    ProductName = line.ProductName,
                    ProductPrice = line.ProductPrice,
                    ProductQuantity = line.ProductQuantity,
                    DiscountPercentage = line.DiscountPercentage,
                    DiscountMinimalQuantity = line.DiscountMinimalQuantity,
                });
            }
            orderInvoice.TotalPrice = orderInvoice.Products.Sum(x => DiscountedPrice(x.ProductPrice, x.DiscountPercentage, x.DiscountMinimalQuantity, x.ProductQuantity ));
            return orderInvoice;
        }
        public DatabaseSaveChangesResponse CreateOrder(NewOrder newOrder)
        {
            if (newOrder.ProductIds.Count() != newOrder.ProductQuantities.Count())
                throw new ApiException(400, "Product IDs count must match product quantities count");
            DatabaseSaveChangesResponse response = new DatabaseSaveChangesResponse();
            Order order = repository.CreateOrder(newOrder.OrderName);
            response = repository.SaveAdformDatabaseChange();
            if (response.StatusCode == 200)
            {
                repository.CreateOrderlines(order.OrderId, newOrder.ProductIds, newOrder.ProductQuantities);
                response = repository.SaveAdformDatabaseChange();
            }
            return response;
        }
        public double DiscountedPrice(double productPrice, int discountPercentage, int minimalQuantity, int productQuantity)
        {
            double price = 0;
            if (discountPercentage != 0 && productQuantity > minimalQuantity)
            {
                price = productQuantity *
                    (productPrice - (productPrice * ((double)discountPercentage / 100)));
            }
            else
                price = productPrice;
            return price;
        }
    }
}
