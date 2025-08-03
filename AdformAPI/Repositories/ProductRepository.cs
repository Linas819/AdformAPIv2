using AdformAPI.AdformDB;
using AdformAPI.Models;
using System.Data.Common;

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
                         join ol in dbContext.Orderlines on p.ProductId equals ol.ProductId into ordline
                         from ordl in ordline.DefaultIfEmpty()
                         where d.DiscountId == discountId && (ordl == null || ordl.ProductQuantity > d.MinimalQuantity)
                         select new DiscountProductOrderLine
                         {
                             ProductName = p.ProductName,
                             DiscountPercentage = d.DiscountPercentage,
                             ProductQuantity = ordl != null ? ordl.ProductQuantity : 0,
                             OrderId = ordl != null ? ordl.OrderId : 0,
                             ProductPrice = p.ProductPrice
                         }).ToList();
            return orderLine;
        }
        public Product CreateProduct(NewProduct newProduct)
        {
            Product product = new Product();
            product.ProductName = newProduct.ProductName;
            product.ProductPrice = newProduct.ProductPrice;
            dbContext.Products.Add(product);
            return product;
        }
        public void CreateProductDiscount(NewProductDiscount newProductDiscount)
        {
            Discount discount = new Discount();
            discount.ProductId = newProductDiscount.ProductId;
            discount.DiscountPercentage = newProductDiscount.DiscountPercentage;
            discount.MinimalQuantity = newProductDiscount.MinimalQuantity;
            dbContext.Discounts.Add(discount);
        }
        public DatabaseSaveChangesResponse SaveAdformDatabaseChange()
        {
            DatabaseSaveChangesResponse response = new DatabaseSaveChangesResponse();
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbException ex)
            {
                response.StatusCode = 400;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
