using AdformAPI.AdformDB;
using AdformAPI.Exceptions;
using AdformAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace AdformAPI.Repositories
{
    public class OrderRepository
    {
        private AdformDatabaseContext dbContext;
        public OrderRepository(AdformDatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IQueryable<Order> GetOrders()
        {
            return dbContext.Orders.AsQueryable<Order>();
        }
        public IQueryable<OrderProductDetail> GetOrderProducts(int orderId, int limit, int pagesize)
        {
            return (from o in dbContext.Orders
                                        join ol in dbContext.OrderLines on o.OrderId equals ol.OrderId
                                        join p in dbContext.Products on ol.ProductId equals p.ProductId
                                        where o.OrderId == orderId
                                        select new OrderProductDetail {
                                            ProductId = p.ProductId,
                                            ProductName = p.ProductName,
                                            ProductPrice = p.ProductPrice,
                                            ProductQuantity = ol.ProductQuantity
                                        }) as IQueryable<OrderProductDetail>;
        }
        public List<OrderLineDetail> GetOrderlines(int orderId)
        {
            return (from o in dbContext.Orders
                         join ol in dbContext.OrderLines on o.OrderId equals ol.OrderId
                         join p in dbContext.Products on ol.ProductId equals p.ProductId
                         join d in dbContext.Discounts on p.ProductId equals d.ProductId into discount
                         from disc in discount.DefaultIfEmpty()
                         where o.OrderId == orderId
                         select new OrderLineDetail
                         {
                             OrderName = o.OrderName,
                             ProductId = p.ProductId,
                             ProductName = p.ProductName,
                             ProductPrice = p.ProductPrice,
                             ProductQuantity = ol.ProductQuantity,
                             DiscountPercentage = disc != null ? (int)disc.DiscountPercentage : 0,
                             DiscountMinimalQuantity = disc != null ? (int)disc.MinimalQuantity : 0
                         }).ToList();
        }
        public Order CreateOrder(string orderName)
        {
            Order order = new Order();
            order.OrderName = orderName;
            dbContext.Orders.Add(order);
            return order;
        }
        public void CreateOrderLines(int orderId, int[] productIds, int[] productQuantities)
        {
            List<OrderLine> orderlines = new List<OrderLine>();
            for (int i = 0; i < productIds.Count(); i++)
            {
                orderlines.Add(new OrderLine
                {
                    OrderId = orderId,
                    ProductId = productIds[i],
                    ProductQuantity = productQuantities[i]
                });
            }
            dbContext.AddRange(orderlines);
        }
        public DatabaseSaveChangesResponse SaveAdformDatabaseChange()
        {
            DatabaseSaveChangesResponse response = new DatabaseSaveChangesResponse();
            try
            {
                dbContext.SaveChanges();
            } catch (DbUpdateException ex){
                response.StatusCode = 803;
                response.Message = ex.InnerException.Message;
                throw new ApiException(response.StatusCode, response.Message);
            }
            return response;
        }
    }
}
