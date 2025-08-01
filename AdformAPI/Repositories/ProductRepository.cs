using AdformAPI.AdformDB;

namespace AdformAPI.Repositories
{
    public class ProductRepository
    {
        private AdformDatabaseContext dbContext;
        public ProductRepository(AdformDatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
