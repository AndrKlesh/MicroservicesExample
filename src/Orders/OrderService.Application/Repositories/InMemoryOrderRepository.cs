using OrderService.Abstractions.Models;
using OrderService.Abstractions.Repositories;

namespace OrderService.Application.Repositories
{
  public class InMemoryOrderRepository : IOrderRepository
  {
    public Task<Order> AddOrderAsync(Order order)
    {
      _orders[order.Id] = order;
      return Task.FromResult(order);
    }

    public Task<bool> DeleteOrderAsync(Guid id)
    {
      return Task.FromResult(_orders.Remove(id));
    }

    public Task<Order?> GetOrderAsync(Guid id)
    {
      _orders.TryGetValue(id, out var order);
      return Task.FromResult(order);

    }

    public Task<IEnumerable<Order>> GetOrdersAsync()
    {
      return Task.FromResult((IEnumerable<Order>)_orders.Values.ToArray());
    }

    private readonly Dictionary<Guid, Order> _orders = [];
  }
}
