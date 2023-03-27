using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;

namespace Times.Infrastructure.Repositories;

public class Auth0AccessTokenProvider : IAccessTokenProvider
{
    private string? CachedAccessToken { get; set; }
    private readonly ILogger<Auth0AccessTokenProvider> _logger;
    private readonly IConfiguration _config;

    public Auth0AccessTokenProvider(IConfiguration config, ILogger<Auth0AccessTokenProvider> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task<string?> GetAccessTokenAsync()
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

            var response = await client.ExecuteAsync<dynamic>(request);

            if (response.IsSuccessful)
            {
                var json = JsonConvert.DeserializeObject<dynamic>(response.Content);
                CachedAccessToken = json.access_token;
            }
            else
            {
                _logger.LogError($"Could not get access token from Auth0. Status Code: {response.StatusCode}, Message: {response.Data?.GetProperty("error_description")}");
            }
        }

        return CachedAccessToken;
    }
}
