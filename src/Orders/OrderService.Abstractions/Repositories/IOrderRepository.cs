using OrderService.Abstractions.Models;

namespace OrderService.Abstractions.Repositories;
public interface IOrderRepository
{
  Task<Order> AddOrderAsync(Order order);
  Task<Order?> GetOrderAsync(Guid id);
  Task<IEnumerable<Order>> GetOrdersAsync();
  Task<bool> DeleteOrderAsync(Guid id);
}
