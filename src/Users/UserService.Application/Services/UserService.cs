using UserService.Abstractions.Models;
using UserService.Abstractions.Repositories;
using UserService.Abstractions.Services;

namespace UserService.Application.Services;
public class UserService : IUserService
{
  public UserService(IUserRepository userRepository)
  {
    _userRepository = userRepository;
  }

  public async Task<User> AddUserAsync(UserForCreate userCreate)
  {
    if (string.IsNullOrWhiteSpace(userCreate.Email))
      throw new ArgumentException("Email cannot be empty");

    var existingUsers = await _userRepository.GetUsersAsync();
    if (existingUsers.Any(u => u.Email.Equals(userCreate.Email, StringComparison.OrdinalIgnoreCase)))
      throw new InvalidOperationException($"User with email {userCreate.Email} already exists");

    var user = new User
    {
      Id = Guid.NewGuid(),
      Email = userCreate.Email,
      FirstName = userCreate.FirstName,
      LastName = userCreate.LastName,
      CreatedAt = DateTime.UtcNow
    };

    return await _userRepository.AddUserAsync(user);
  }

  public Task<User?> GetUserAsync(Guid id) => _userRepository.GetUserAsync(id);
  public Task<IEnumerable<User>> GetUsersAsync() => _userRepository.GetUsersAsync();
  public Task<bool> DeleteUserAsync(Guid id) => _userRepository.DeleteUserAsync(id);


  private readonly IUserRepository _userRepository;
}
