using AdformAPI.AdformDB;
using AdformAPI.Models;
using System.Collections.Generic;
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
        public List<OrderLineDetail> GetOrderlines(int orderId)
        {
            List <OrderLineDetail> orderLineDetails = (from o in dbContext.Orders
                         join ol in dbContext.Orderlines on o.OrderId equals ol.OrderId
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
            return orderLineDetails;
        }
        public string SaveAdformDatabaseChange(string message)
        {
            try
            {
                dbContext.SaveChanges();
            } catch (DbException ex){
                message = ex.Message;
            }
            return message;
        }
    }
}
