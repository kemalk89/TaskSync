using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using TaskSync.Infrastructure.Repositories;

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
                // Remove the original implementation and add our mock
                services.RemoveAll<IExternalUserRepository>();
                services.AddSingleton<IExternalUserRepository>(new MockExternalUserRepository());
                
                services.Configure<AuthenticationOptions>(options =>
                {
                    options.DefaultAuthenticateScheme = "TestScheme";
                    options.DefaultChallengeScheme = "TestScheme";
                    options.DefaultScheme = "TestScheme";
                });
                
                // Add test authentication handler
                services.AddAuthentication("TestScheme")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });
                services.AddAuthorization(options =>
                {
                    var authPolicy = new AuthorizationPolicyBuilder("TestScheme")
                        .RequireAuthenticatedUser()
                        .Build();
        
                    options.DefaultPolicy = authPolicy;
                });
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


// Mock implementation for testing
public class MockExternalUserRepository : IExternalUserRepository
{
    public Task<Domain.User.User[]> SearchUsersAsync(string searchText)
    {
        return Task.FromResult(Array.Empty<Domain.User.User>());
    }

    public Task<Domain.User.User[]> FindUsersAsync(string[] userIds)
    {
        return Task.FromResult(Array.Empty<Domain.User.User>());
    }

    public Task<Domain.User.User?> FindUserByIdAsync(string userId)
    {
        return Task.FromResult<Domain.User.User?>(null);
    }

    public Task<Domain.User.User[]> FindUsersAsync(int pageNumber, int pageSize)
    {
        return Task.FromResult(Array.Empty<Domain.User.User>());
    }

    public Task<Domain.User.User?> FindUserByIdFromExternalSourceAsync(string externalUserId)
    {
        if (externalUserId == "non_existent_external_id")
        {
            return Task.FromResult<Domain.User.User?>(null);
        }
        
        var user = new Domain.User.User
        {
            Id = 1,
            Email = "test@example.com",
            Username = "testuser",
            Picture = "https://example.com/picture.jpg",
            ExternalUserId = externalUserId
        };
        return Task.FromResult<Domain.User.User?>(user);
    }
}

