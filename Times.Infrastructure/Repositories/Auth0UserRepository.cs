using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using Times.Domain.Shared;

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

    public async Task<User[]> FindUsersAsync(string[] userIds)
    {
        if (userIds.Length == 0)
        {
            return Array.Empty<User>();
        }

        var users = await getUsersAsync(userIds);

        var result = new List<User>();
        foreach (JsonElement u in users)
        {
            result.Add(MapToUser(u));
        }

        return result.ToArray();
    }

    public async Task<User?> FindUserByIdAsync(string userId)
    {
        var accessToken = await _accessTokenProvider.GetAccessTokenAsync();

        var domain = _config["Auth:MachineToMachineApplication:Domain"];

        var client = new RestClient($"https://{domain}/api/v2");
        client.AddDefaultHeader("Authorization", $"Bearer {accessToken}");

        var request = new RestRequest($"users/{userId}", Method.Get);
        var response = await client.ExecuteAsync<dynamic>(request);
        if (response.IsSuccessful)
        {
            return MapToUser(response.Data);
        }

        _logger.LogWarning($"Could not fetch user by id. Status Code: {response.StatusCode}, Message: {response.Data?.GetProperty("message")}");
        return null;
    }

    private User MapToUser(JsonElement user)
    {
        return new User
        {
            Id = user.GetProperty("user_id").ToString(),
            Username = user.GetProperty("name").ToString(),
            Email = user.GetProperty("email").ToString(),
            Picture = user.GetProperty("picture").ToString()
        };
    }

    private async Task<dynamic[]> getUsersAsync(string[] userIds)
    {
        if (userIds.Length == 0)
        {
            return Array.Empty<dynamic>();
        }

        var accessToken = await _accessTokenProvider.GetAccessTokenAsync();

        var domain = _config["Auth:MachineToMachineApplication:Domain"];

        var client = new RestClient($"https://{domain}/api/v2");
        client.AddDefaultHeader("Authorization", $"Bearer {accessToken}");

        var queryParams = $"?fields=user_id,email,picture,name&include_fields=true&q=";
        queryParams += string.Join(" OR ", userIds);

        var request = new RestRequest($"users{queryParams}", Method.Get);
        var response = await client.ExecuteAsync<dynamic>(request);
        if (response.IsSuccessful)
        {
            return response.Data ?? Array.Empty<dynamic>();
        }

        _logger.LogWarning($"Could not fetch users by ids. Status Code: {response.StatusCode}, Message: {response.Data?.GetProperty("message")}");
        return Array.Empty<dynamic>();
    }
}
