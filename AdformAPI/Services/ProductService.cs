using AdformAPI.Repositories;

namespace AdformAPI.Services
{
    public class ProductService
    {
        private ProductRepository repository;
        public ProductService(ProductRepository repository)
        {
            this.repository = repository;
        }
    }
}
