using AdformAPI.AdformDB;
using AdformAPI.Models;
using AdformAPI.Repositories;

namespace AdformAPI.GraphQL
{
    public class CreateProductMutation
    {
        private ProductRepository repository;
        public CreateProductMutation(ProductRepository repository)
        {
            this.repository = repository;
        }
        public Product CreateProduct(NewProduct newProduct)
        {
            Product product = repository.CreateProduct(newProduct);
            repository.SaveAdformDatabaseChange();
            return product;
        }
    }
}
