using UserService.Abstractions.Models;
using UserService.Abstractions.Repositories;

namespace UserService.Application.Repositories;
public class InMemoryUserRepository : IUserRepository
{
  public Task<User> AddUserAsync(User user)
  {
    _users[user.Id] = user;
    return Task.FromResult(user);
  }

  public Task<User?> GetUserAsync(Guid id)
  {
    _users.TryGetValue(id, out var user);
    return Task.FromResult(user);
  }

  public Task<IEnumerable<User>> GetUsersAsync()
  {
    return Task.FromResult(_users.Values.AsEnumerable());
  }

  public Task<bool> DeleteUserAsync(Guid id)
  {
    return Task.FromResult(_users.Remove(id));
  }

  private readonly Dictionary<Guid, User> _users = [];
}
