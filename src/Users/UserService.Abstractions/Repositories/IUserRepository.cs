using UserService.Abstractions.Models;

namespace UserService.Abstractions.Repositories;
public interface IUserRepository
{
  Task<User> AddUserAsync(User user);
  Task<User?> GetUserAsync(Guid id);
  Task<IEnumerable<User>> GetUsersAsync();
  Task<bool> DeleteUserAsync(Guid id);
}
