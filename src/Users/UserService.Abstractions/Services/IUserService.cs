using UserService.Abstractions.Models;

namespace UserService.Abstractions.Services;
public interface IUserService
{
  Task<User> AddUserAsync(UserForCreate userCreate);
  Task<User?> GetUserAsync(Guid id);
  Task<IEnumerable<User>> GetUsersAsync();
  Task<bool> DeleteUserAsync(Guid id);
}
