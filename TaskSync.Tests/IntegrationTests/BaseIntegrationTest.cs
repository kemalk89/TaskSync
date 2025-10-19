using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
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

    protected async Task AssertEndpointsReturnUnauthorized((string Url, HttpMethod Method, object? Payload)[] endpoints)
    {
        foreach (var (url, method, payload) in endpoints)
        {
            HttpResponseMessage response;
            if (method == HttpMethod.Post)
            {
                response = await _client.PostAsJsonAsync(url, payload);
            } else if (method == HttpMethod.Get)
            {
                response = await _client.GetAsync(url);
            
            }
            else
            {
                throw new NotSupportedException($"Method {method} not supported");
            }

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}

