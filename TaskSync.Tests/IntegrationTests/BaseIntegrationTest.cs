using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace TaskSync.Tests.IntegrationTests;

public class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    protected readonly HttpClient _client;
    protected readonly IntegrationTestWebAppFactory _factory;
    
    public BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _factory = factory;
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.Configure<AuthenticationOptions>(options =>
                {
                    options.DefaultAuthenticateScheme = "TestScheme";
                    options.DefaultChallengeScheme = "TestScheme";
                    options.DefaultScheme = "TestScheme";
                });
                
                // Add test authentication handler
                services.AddAuthentication("TestScheme")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });
            });
        }).CreateClient();
    }
    
    /// <summary>
    /// Sets Authorization Header for requests.
    /// </summary>
    /// <param name="roles"></param>
    /// <param name="externalUserId"></param>
    protected void SetAuthenticatedUser(string[]? roles = null, string externalUserId = "integration_tests|01")
    {
        roles ??= ["User"];
        
        var authData = new TestAuthData
        {
            Roles = roles,
            UserName = externalUserId
        };

        var json = JsonSerializer.Serialize(authData);
        var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme", encoded);
    }
}

