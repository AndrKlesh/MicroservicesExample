using OrderService.Abstractions.Models;

namespace OrderService.Abstractions.Services;
public interface IOrderService
{
  Task<Order> AddOrderAsync(OrderForCreate OrderCreate);
  Task<Order?> GetOrderAsync(Guid id);
  Task<IEnumerable<Order>> GetOrdersAsync();
  Task<bool> DeleteOrderAsync(Guid id);
}
