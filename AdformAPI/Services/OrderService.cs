using AdformAPI.AdformDB;
using AdformAPI.Exceptions;
using AdformAPI.Models;
using AdformAPI.Repositories;
using Microsoft.EntityFrameworkCore;

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
            int orderLimit = page * pageSize;
            int productLimit = productPage * productPageSize;
            List<OrderDetail> orders = new List<OrderDetail>();
            IQueryable<Order> ordsQuery = repository.GetOrders();
            List<Order> ords = orderLimit == 0
                ? ordsQuery.ToList()
                : ordsQuery
                    .Skip(orderLimit - pageSize) // Skips all the unssecary data
                    .Take(pageSize)
                    .ToList();
            if (ords.Count() == 0)
                throw new ApiException(404, "No orders found");
            foreach (Order ord in ords)
            {
                OrderDetail order = new OrderDetail();
                IQueryable<OrderProductDetail> orderProductsQuery = repository.GetOrderProducts(ord.OrderId, productPage * productPageSize, productPageSize);
                List<OrderProductDetail> orderProducts = productLimit == 0
                    ? orderProductsQuery.ToList()
                    : orderProductsQuery
                        .Skip(productLimit - productPageSize) // Skips all the unssecary data
                        .Take(productPageSize)
                        .ToList();
                order.OrderId = ord.OrderId;
                order.OrderName = ord.OrderName;
                order.OrderProducts = orderProducts;
                orders.Add(order);
            }
            return orders;
        }
        public OrderInvoice GetOrderInvoice(int orderId)
        {
            List<OrderLineDetail> orderLineDetails = repository.GetOrderlines(orderId);
            if (orderLineDetails.Count() == 0)
                throw new ApiException(404, "Order Id: " + orderId + " has no products");
            OrderInvoice orderInvoice = new OrderInvoice();
            orderInvoice.OrderId = orderId;
            orderInvoice.OrderName = orderLineDetails.First().OrderName;
            foreach (OrderLineDetail line in orderLineDetails)
            {
                orderInvoice.Products.Add(new OrderInvoiceProductDetail
                {
                    ProductId = line.ProductId,
                    ProductName = line.ProductName,
                    ProductPrice = line.ProductPrice,
                    ProductQuantity = line.ProductQuantity,
                    DiscountPercentage = line.DiscountPercentage,
                    DiscountMinimalQuantity = line.DiscountMinimalQuantity,
                });
            }
            orderInvoice.TotalPrice = orderInvoice.Products.Sum(x => DiscountedPrice(x.ProductPrice, x.DiscountPercentage, x.DiscountMinimalQuantity, x.ProductQuantity));
            return orderInvoice;
        }
        public DatabaseSaveChangesResponse CreateOrder(NewOrder newOrder)
        {
            if (newOrder.ProductIds.Count() != newOrder.ProductQuantities.Count())
                throw new ApiException(400, "Product IDs count must match product quantities count");
            if (newOrder.ProductIds.Count() == 0)
                throw new ApiException(400, "No product IDs provided");
            if (newOrder.ProductQuantities.Count() == 0)
                throw new ApiException(400, "No product quantities provided");
            if (newOrder.OrderName == string.Empty)
                throw new ApiException(400, "Order name required");
            DatabaseSaveChangesResponse response = new DatabaseSaveChangesResponse();
            Order order = repository.CreateOrder(newOrder.OrderName);
            response = repository.SaveAdformDatabaseChange();
            if (response.StatusCode == 200)
            {
                repository.CreateOrderLines(order.OrderId, newOrder.ProductIds, newOrder.ProductQuantities);
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
