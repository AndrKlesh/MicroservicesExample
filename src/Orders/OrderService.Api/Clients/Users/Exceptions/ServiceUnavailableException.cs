namespace OrderService.Api.Clients.Users.Exceptions;
public class ServiceUnavailableException : Exception
{
  public ServiceUnavailableException(string message, Exception inner)
      : base(message, inner) { }

  public ServiceUnavailableException() : base()
  {
  }

  public ServiceUnavailableException(string? message) : base(message)
  {
  }
}
