using Microsoft.AspNetCore.Mvc;
using UserService.Abstractions.Models;
using UserService.Abstractions.Services;
using UserService.Api.Models;

namespace UserService.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class UsersController : ControllerBase
  {
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
      _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers()
    {
      var users = await _userService.GetUsersAsync();
      var response = users.Select(MapToResponse);
      return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserResponse>> GetUser(Guid id)
    {
      var user = await _userService.GetUserAsync(id);
      if (user == null) return NotFound();
      return Ok(MapToResponse(user));
    }

    [HttpPost]
    public async Task<ActionResult<UserResponse>> CreateUser(UserForCreateRequest request)
    {
      try
      {
        var userCreate = new UserForCreate
        {
          Email = request.Email,
          FirstName = request.FirstName,
          LastName = request.LastName
        };

        var user = await _userService.AddUserAsync(userCreate);
        var response = MapToResponse(user);
        return CreatedAtAction(nameof(GetUser), new { id = response.Id }, response);
      }
      catch (ArgumentException ex)
      {
        return BadRequest(ex.Message);
      }
      catch (InvalidOperationException ex)
      {
        return Conflict(ex.Message);
      }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
      var deleted = await _userService.DeleteUserAsync(id);
      if (!deleted) return NotFound();
      return NoContent();
    }

    private static UserResponse MapToResponse(User user) => new()
    {
      Id = user.Id,
      Email = user.Email,
      FirstName = user.FirstName,
      LastName = user.LastName,
      CreatedAt = user.CreatedAt
    };
  }
}
