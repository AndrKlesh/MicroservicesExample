namespace OrderService.Api.Models;

public class OrderResponse
{
  public Guid Id { get; set; }
  public Guid UserId { get; set; }
  public double TotalSum { get; set; }
  public DateTime CreatedAt { get; set; }
}

