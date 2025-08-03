using AdformAPI.AdformDB;
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
        public OrderInvoice GetOrderInvoice(int orderId)
        {
            List<OrderlineDetail> orderLineDetails = repository.GetOrderlines(orderId);
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
            DatabaseSaveChangesResponse response = new DatabaseSaveChangesResponse();
            Order order = repository.CreateOrder(newOrder.OrderName);
            response = repository.SaveAdformDatabaseChange();
            repository.CreateOrderlines(order.OrderId, newOrder.ProductIds, newOrder.ProductQuantities);
            response = repository.SaveAdformDatabaseChange();
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
