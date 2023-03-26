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
    private readonly ILogger<Auth0UserRepository> _logger;
    private readonly IConfiguration _config;
    private string? CachedAccessToken { get; set; }

    public Auth0UserRepository(IConfiguration config, ILogger<Auth0UserRepository> logger)
    {
        _config = config;
        _logger = logger;
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

    public async Task<User> FindUserByIdAsync(string userId)
    {
        var user = await GetUserAsync(userId);
        return MapToUser(user);
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

    private async Task<dynamic> GetUserAsync(string userId)
    {
        var accessToken = await GetAccessTokenAsync();

        var domain = _config["Auth:MachineToMachineApplication:Domain"];

        var client = new RestClient($"https://{domain}/api/v2");
        client.AddDefaultHeader("Authorization", $"Bearer {accessToken}");

        var request = new RestRequest($"users/{userId}", Method.Get);
        var response = await client.ExecuteAsync<dynamic>(request);
        return response.Data;
    }

    private async Task<dynamic[]> getUsersAsync(string[] userIds)
    {
        var accessToken = await GetAccessTokenAsync();

        var domain = _config["Auth:MachineToMachineApplication:Domain"];

        var client = new RestClient($"https://{domain}/api/v2");
        client.AddDefaultHeader("Authorization", $"Bearer {accessToken}");

        var queryParams = $"?fields=user_id,email,picture,name&include_fields=true&q=";
        queryParams += string.Join(" OR ", userIds);

        var request = new RestRequest($"users{queryParams}", Method.Get);
        var response = await client.ExecuteAsync<dynamic[]>(request);
        return response.Data;
    }

    private async Task<string> GetAccessTokenAsync()
    {
        if (CachedAccessToken == null)
        {
            var domain = _config["Auth:MachineToMachineApplication:Domain"];
            var clientId = _config["Auth:MachineToMachineApplication:ClientId"];
            var clientSecret = _config["Auth:MachineToMachineApplication:ClientSecret"];

            var client = new RestClient($"https://{domain}/");
            var request = new RestRequest("oauth/token", Method.Post);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter(
                "application/x-www-form-urlencoded",
                $"grant_type=client_credentials&client_id={clientId}&client_secret={clientSecret}&audience=https%3A%2F%2F{domain}%2Fapi%2Fv2%2F",
                ParameterType.RequestBody);

            var response = await client.ExecuteAsync(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _logger.LogError("Could not get access token from Auth0 because Auth0 returned Unauthorized.");
                return null;
            }

            var json = JsonConvert.DeserializeObject<dynamic>(response.Content);
            CachedAccessToken = json.access_token;
        }

        return CachedAccessToken;
    }
}
