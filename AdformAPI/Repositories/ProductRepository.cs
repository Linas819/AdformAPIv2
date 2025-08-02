using AdformAPI.AdformDB;
using AdformAPI.Models;
using System.Data.Common;
using System.Linq;

namespace AdformAPI.Repositories
{
    public class ProductRepository
    {
        private AdformDatabaseContext dbContext;
        public ProductRepository(AdformDatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<ProductDetail> GetProducts(string productName, int limit)
        {
            List<ProductDetail> products = new List<ProductDetail>();
            var productsQueryable = (from p in dbContext.Products
                                     where string.IsNullOrEmpty(productName) || p.ProductName.ToLower().Contains(productName.ToLower())
                                     select new ProductDetail
                                        {
                                            ProductId = p.ProductId,
                                            ProductName = p.ProductName,
                                            ProductPrice = p.ProductPrice
                                        }) as IQueryable<ProductDetail>;
            products = limit != 0 ? productsQueryable.Take(limit).ToList() : productsQueryable.ToList();
            return products;
        }
        public List<DiscountProductOrderLine> GetDiscountProductOrderLines(int discountId)
        {
            List<DiscountProductOrderLine> orderLine = (from d in dbContext.Discounts
                         join p in dbContext.Products on d.ProductId equals p.ProductId
                         join ol in dbContext.Orderlines on p.ProductId equals ol.ProductId
                         where d.DiscountId == discountId && ol.ProductQuantity > d.MinimalQuantity
                         select new DiscountProductOrderLine
                         {
                             ProductName = p.ProductName,
                             DiscountPercentage = d.DiscountPercentage,
                             ProductQuantity = ol.ProductQuantity,
                             OrderId = ol.OrderId,
                             ProductPrice = p.ProductPrice

                         }).ToList();
            return orderLine;
        }
        public void CreateProduct(NewProduct newProduct)
        {
            Product product = new Product();
            product.ProductName = newProduct.ProductName;
            product.ProductPrice = newProduct.ProductPrice;
            dbContext.Products.Add(product);
        }
        public void CreateProductDiscount(NewProductDiscount newProductDiscount)
        {
            Discount discount = new Discount();
            discount.ProductId = newProductDiscount.ProductId;
            discount.DiscountPercentage = newProductDiscount.DiscountPercentage;
            discount.MinimalQuantity = newProductDiscount.MinimalQuantity;
            dbContext.Discounts.Add(discount);
        }
        public string SaveAdformDatabaseChange(string message)
        {
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbException ex)
            {
                message = ex.Message;
            }
            return message;
        }
    }
}
