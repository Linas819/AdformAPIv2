using AdformAPI.Repositories;

namespace AdformAPI.Services
{
    public class OrderService
    {
        private OrderRepository repository;
        public OrderService(OrderRepository repository)
        {
            this.repository = repository;
        }
    }
}
