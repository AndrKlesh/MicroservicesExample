using OrderService.Abstractions.Models;
using OrderService.Abstractions.Repositories;
using OrderService.Abstractions.Services;
using UserService.Abstractions.Services;

namespace OrderService.Application.Services;
public class OrderService : IOrderService
{
  public OrderService(IOrderRepository orderRepository, IUserService userService)
  {
    _orderRepository = orderRepository;
    _userService = userService;
  }

  public async Task<Order> AddOrderAsync(OrderForCreate orderforCreate)
  {
    var user = await _userService.GetUserAsync(orderforCreate.UserId);
    if (user == null)
      throw new ArgumentException($"User with id {orderforCreate.UserId} not found");

    var order = new Order
    {
      Id = Guid.NewGuid(),
      UserId = orderforCreate.UserId,
      TotalSum = Random.Shared.NextDouble() * 1000,
      CreatedAt = DateTime.UtcNow
    };

    return await _orderRepository.AddOrderAsync(order);
  }

  public Task<bool> DeleteOrderAsync(Guid id)
  {
    return _orderRepository.DeleteOrderAsync(id);
  }

  public Task<Order?> GetOrderAsync(Guid id)
  {
    return _orderRepository.GetOrderAsync(id);
  }

  public Task<IEnumerable<Order>> GetOrdersAsync()
  {
    return _orderRepository.GetOrdersAsync();
  }

  private readonly IOrderRepository _orderRepository;
  private readonly IUserService _userService;
}

