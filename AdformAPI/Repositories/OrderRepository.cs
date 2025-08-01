using AdformAPI.AdformDB;

namespace AdformAPI.Repositories
{
    public class OrderRepository
    {
        private AdformDatabaseContext dbContext;
        public OrderRepository(AdformDatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
