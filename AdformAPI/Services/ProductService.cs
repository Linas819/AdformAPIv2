﻿using AdformAPI.Models;
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
            int limit = page * pageSize;
            List<ProductDetail> products = productRepository.GetProducts(productName, limit);
            if (limit != 0)
            {
                int productsCount = products.Count;
                int totalPages = (int)Math.Ceiling((decimal)productsCount / pageSize);
                products = products
                    .Skip((page -1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            return products;

        }
        public ProductDiscount GetProductDiscount(int discountId)
        {
            List<DiscountProductOrderLine> orderLines = productRepository.GetDiscountProductOrderLines(discountId);
            ProductDiscount productDiscount = new ProductDiscount();
            productDiscount.ProductName = orderLines.First().ProductName;
            productDiscount.DiscountPercentage = orderLines.First().DiscountPercentage;
            productDiscount.OrderCount = orderLines.Select(x => x.OrderId).Distinct().Count();
            productDiscount.TotalAmount = orderLines.Sum(x => x.ProductQuantity * (x.ProductPrice - 
                (x.ProductPrice * ((double)x.DiscountPercentage / 100))));
            return productDiscount;
        }
        public string CreateProduct(NewProduct newProduct)
        {
            string message = "Product created";
            productRepository.CreateProduct(newProduct);
            message = productRepository.SaveAdformDatabaseChange(message);
            return message;
        }
        public string CreateProductDiscount(NewProductDiscount newProductDiscount)
        {
            string message = "Product discount created";
            productRepository.CreateProductDiscount(newProductDiscount);
            message = productRepository.SaveAdformDatabaseChange(message);
            return message;
        }
    }
}
