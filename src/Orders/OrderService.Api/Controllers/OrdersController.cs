using Microsoft.AspNetCore.Mvc;
using OrderService.Abstractions.Models;
using OrderService.Abstractions.Services;
using OrderService.Api.Models;

namespace OrderService.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class OrdersController : ControllerBase
  {
    private readonly IOrderService _orderService;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
    {
      _orderService = orderService;
      _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrders()
    {
      try
      {
        var orders = await _orderService.GetOrdersAsync();
        var response = orders.Select(MapToResponse);
        return Ok(response);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error while getting orders");
        return StatusCode(500, "Internal server error");
      }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderResponse>> GetOrder(Guid id)
    {
      try
      {
        var order = await _orderService.GetOrderAsync(id);
        if (order == null)
          return NotFound($"Order with ID {id} not found");

        return Ok(MapToResponse(order));
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error while getting order {OrderId}", id);
        return StatusCode(500, "Internal server error");
      }
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponse>> CreateOrder(OrderCreateRequest request)
    {
      try
      {
        var orderCreate = new OrderForCreate
        {
          UserId = request.UserId
        };

        var order = await _orderService.AddOrderAsync(orderCreate);
        var response = MapToResponse(order);

        return CreatedAtAction(nameof(GetOrder), new { id = response.Id }, response);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error while creating order for user {UserId}", request.UserId);
        return StatusCode(500, "Internal server error");
      }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteOrder(Guid id)
    {
      try
      {
        var deleted = await _orderService.DeleteOrderAsync(id);
        if (!deleted)
          return NotFound($"Order with ID {id} not found");

        return NoContent();
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error while deleting order {OrderId}", id);
        return StatusCode(500, "Internal server error");
      }
    }

    private static OrderResponse MapToResponse(Order order) => new()
    {
      Id = order.Id,
      UserId = order.UserId,
      TotalSum = order.TotalSum,
      CreatedAt = order.CreatedAt
    };
  }
}
