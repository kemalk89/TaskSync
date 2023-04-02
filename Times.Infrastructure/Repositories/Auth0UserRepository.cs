using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using Times.Domain.User;

namespace Times.Infrastructure.Repositories;

public class Auth0UserRepository : IUserRepository
{
    private readonly IAccessTokenProvider _accessTokenProvider;
    private readonly ILogger<Auth0UserRepository> _logger;
    private readonly IConfiguration _config;

    public Auth0UserRepository(IConfiguration config, ILogger<Auth0UserRepository> logger, IAccessTokenProvider accessTokenProvider)
    {
        _config = config;
        _logger = logger;
        _accessTokenProvider = accessTokenProvider;
    }

    public async Task<User[]> SearchUsersAsync(string searchText)
    {
        if (String.IsNullOrEmpty(searchText))
        {
            return Array.Empty<User>();
        }

        var users = await SearchUsersBySearchTextAsync(searchText);

        var result = new List<User>();
        foreach (Auth0UserResponse u in users)
        {
            result.Add(MapToUser(u));
        }

        return result.ToArray();
    }

    public async Task<User[]> FindUsersAsync(string[] userIds)
    {
        if (userIds.Length == 0)
        {
            return Array.Empty<User>();
        }

        var users = await GetUsersAsync(userIds);

        var result = new List<User>();
        foreach (Auth0UserResponse u in users)
        {
            result.Add(MapToUser(u));
        }

        return result.ToArray();
    }

    public async Task<User?> FindUserByIdAsync(string userId)
    {
        var client = await GetRestClient();

        var request = new RestRequest($"users/{userId}", Method.Get);
        var response = await client.ExecuteAsync<Auth0UserResponse>(request);
        if (response.IsSuccessful)
        {
            var data = response.Data;
            return data == null ? null : MapToUser(data);
        }

        _logger.LogWarning($"Could not fetch user by id. Status Code: {response.StatusCode}, Message: {response.ErrorMessage}");
        return null;
    }

    private async Task<Auth0UserResponse[]> GetUsersAsync(string[] userIds)
    {
        var client = await GetRestClient();

        var queryParams = $"?fields=user_id,email,picture,name&include_fields=true&q=";
        queryParams += string.Join(" OR ", userIds);

        var request = new RestRequest($"users{queryParams}", Method.Get);
        var response = await client.ExecuteAsync<Auth0UserResponse[]>(request);
        if (response.IsSuccessful)
        {
            return response.Data ?? Array.Empty<Auth0UserResponse>();
        }

        _logger.LogWarning($"Could not fetch users by ids. Status Code: {response.StatusCode}, Message: {response.ErrorMessage}");
        return Array.Empty<Auth0UserResponse>();
    }

    private async Task<Auth0UserResponse[]> SearchUsersBySearchTextAsync(string searchText)
    {
        var client = await GetRestClient();

        var queryParams = $"?fields=user_id,email,picture,name&include_fields=true&q=";
        queryParams += $"name:{searchText}";

        var request = new RestRequest($"users{queryParams}*", Method.Get);
        var response = await client.ExecuteAsync<Auth0UserResponse[]>(request);
        if (response.IsSuccessful)
        {
            return response.Data ?? Array.Empty<Auth0UserResponse>();
        }

        _logger.LogWarning($"Could not search users. Status Code: {response.StatusCode}, Message: {response.ErrorMessage}");
        return Array.Empty<Auth0UserResponse>();
    }

    private async Task<RestClient> GetRestClient()
    {
        var accessToken = await _accessTokenProvider.GetAccessTokenAsync();

        var domain = _config["Auth:MachineToMachineApplication:Domain"];

        var client = new RestClient($"https://{domain}/api/v2");
        client.AddDefaultHeader("Authorization", $"Bearer {accessToken}");

        return client;
    }

    private User MapToUser(Auth0UserResponse user)
    {
        return new User
        {
            Id = user.user_id,
            Username = user.name,
            Email = user.email,
            Picture = user.picture
        };
    }
}
