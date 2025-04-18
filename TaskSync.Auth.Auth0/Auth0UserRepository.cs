using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TaskSync.Domain.User;
using RestSharp;
using TaskSync.Infrastructure.Repositories;

namespace TaskSync.Auth.Auth0;

public class Auth0UserRepository : IExternalUserRepository
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
            return [];
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

        var request = new RestRequest($"users/{userId}");
        var response = await client.ExecuteAsync<Auth0UserResponse>(request);
        if (response.IsSuccessful)
        {
            var data = response.Data;
            return data == null ? null : MapToUser(data);
        }

        _logger.LogWarning($"Could not fetch user by id. Status Code: {response.StatusCode}, Message: {response.ErrorMessage}");
        return null;
    }

    public async Task<User?> FindUserByIdFromExternalSourceAsync(string externalUserId)
    {
        var client = await GetRestClient();

        var request = new RestRequest($"users/{externalUserId}");
        var response = await client.ExecuteAsync<Auth0UserResponse>(request);
        if (response.IsSuccessful)
        {
            var data = response.Data;
            return data == null ? null : MapToUser(data);
        }

        _logger.LogWarning($"Could not fetch user by id. Status Code: {response.StatusCode}, Message: {response.ErrorMessage}");
        return null;
    }

    public async Task<User[]> FindUsersAsync(int pageNumber = 1, int pageSize = 50)
    {
        if (pageSize > 50)
        {
            throw new ArgumentOutOfRangeException("Max allowed pageSize is 50!");
        }

        var client = await GetRestClient();

        var queryParams = $"?page={pageNumber - 1}&per_page={pageSize}&q=";

        var request = new RestRequest($"users{queryParams}");
        var response = await client.ExecuteAsync<Auth0UserResponse[]>(request);
        if (response.IsSuccessful && response.Data != null)
        {
            var result = new List<User>();
            foreach (Auth0UserResponse u in response.Data)
            {
                result.Add(MapToUser(u));
            }

            return result.ToArray();
        }
        
        _logger.LogError($"Could not load users from Auth0. HTTP Status Code: {response.StatusCode}. Ensure Client is authorized for Auth0 Management API with permissions: read:users");

        return Array.Empty<User>();
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

        var client = new RestClient($"{domain}/api/v2");
        client.AddDefaultHeader("Authorization", $"Bearer {accessToken}");

        return client;
    }

    private User MapToUser(Auth0UserResponse user)
    {
        return new User
        {
            ExternalUserId = user.user_id,
            Username = user.name,
            Email = user.email,
            Picture = user.picture
        };
    }

}
