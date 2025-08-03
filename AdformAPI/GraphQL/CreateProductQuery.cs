using AdformAPI.AdformDB;
using AdformAPI.Models;
using AdformAPI.Repositories;

namespace AdformAPI.GraphQL
{
    public class CreateProductQuery
    {
        private ProductRepository repository;
        public CreateProductQuery(ProductRepository repository)
        {
            this.repository = repository;
        }
        public Product CreateProduct(NewProduct newProduct)
        {
            Product product = repository.CreateProduct(newProduct);
            repository.SaveAdformDatabaseChange("");
            return product;
        }
    }
}
