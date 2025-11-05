using OrderService.Api.Clients.Users.Exceptions;
using System.Text;
using System.Text.Json;
using UserService.Abstractions.Models;
using UserService.Abstractions.Services;

namespace OrderService.Api.Clients.Users;
public class UserServiceHttpClient : IUserService
{
  private readonly HttpClient _httpClient;
  private readonly JsonSerializerOptions _jsonOptions;

  public UserServiceHttpClient(HttpClient httpClient)
  {
    _httpClient = httpClient;

    _jsonOptions = new JsonSerializerOptions
    {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
      WriteIndented = true
    };
  }

  public async Task<User> AddUserAsync(UserForCreate userCreate)
  {
    try
    {
      var json = JsonSerializer.Serialize(userCreate, _jsonOptions);
      var content = new StringContent(json, Encoding.UTF8, "application/json");

      var response = await _httpClient.PostAsync("api/users", content);

      if (!response.IsSuccessStatusCode)
      {
        await HandleErrorResponse(response, "Failed to create user");
      }

      var responseJson = await response.Content.ReadAsStringAsync();
      var createdUser = JsonSerializer.Deserialize<User>(responseJson, _jsonOptions);

      if (createdUser == null)
        throw new InvalidOperationException("Failed to deserialize created user");

      return createdUser;
    }
    catch (HttpRequestException ex)
    {
      throw new ServiceUnavailableException("User service is unavailable", ex);
    }
  }

  public async Task<User?> GetUserAsync(Guid id)
  {
    try
    {
      var response = await _httpClient.GetAsync($"api/users/{id}");

      if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        return null;

      if (!response.IsSuccessStatusCode)
      {
        await HandleErrorResponse(response, "Failed to get user");
      }

      var responseJson = await response.Content.ReadAsStringAsync();
      var user = JsonSerializer.Deserialize<User>(responseJson, _jsonOptions);

      return user;
    }
    catch (HttpRequestException ex)
    {
      throw new ServiceUnavailableException("User service is unavailable", ex);
    }
  }

  public async Task<IEnumerable<User>> GetUsersAsync()
  {
    try
    {
      var response = await _httpClient.GetAsync("api/users");

      if (!response.IsSuccessStatusCode)
      {
        await HandleErrorResponse(response, "Failed to get users");
      }

      var responseJson = await response.Content.ReadAsStringAsync();
      var users = JsonSerializer.Deserialize<List<User>>(responseJson, _jsonOptions);

      return users ?? Enumerable.Empty<User>();
    }
    catch (HttpRequestException ex)
    {
      throw new ServiceUnavailableException("User service is unavailable", ex);
    }
  }

  public async Task<bool> DeleteUserAsync(Guid id)
  {
    try
    {
      var response = await _httpClient.DeleteAsync($"api/users/{id}");

      if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        return false;

      if (!response.IsSuccessStatusCode)
      {
        await HandleErrorResponse(response, "Failed to delete user");
      }

      return true;
    }
    catch (HttpRequestException ex)
    {
      throw new ServiceUnavailableException("User service is unavailable", ex);
    }
  }

  private async Task HandleErrorResponse(HttpResponseMessage response, string baseMessage)
  {
    var errorContent = await response.Content.ReadAsStringAsync();

    throw response.StatusCode switch
    {
      System.Net.HttpStatusCode.BadRequest =>
          new ArgumentException($"Bad request: {errorContent}"),
      System.Net.HttpStatusCode.Conflict =>
          new InvalidOperationException($"Conflict: {errorContent}"),
      System.Net.HttpStatusCode.NotFound =>
          new KeyNotFoundException($"Resource not found: {errorContent}"),
      _ => new HttpRequestException(
          $"{baseMessage}. Status: {response.StatusCode}, Content: {errorContent}")
    };
  }
}
