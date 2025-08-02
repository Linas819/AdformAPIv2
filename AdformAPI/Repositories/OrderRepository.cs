using AdformAPI.AdformDB;
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
