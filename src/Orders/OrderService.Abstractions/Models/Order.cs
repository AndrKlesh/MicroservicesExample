namespace OrderService.Abstractions.Models
{
  public class Order
  {
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public double TotalSum { get; set; }
    public DateTime CreatedAt { get; set; }
  }
}
