using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using TaskSync.Infrastructure.Repositories;
using TaskSync.Tests.IntegrationTests.User;

namespace TaskSync.Tests.IntegrationTests;

/// <summary>
/// Base class for integration tests.
/// </summary>
public class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    protected readonly HttpClient _client;
    protected readonly TestJwtGenerator _testJwtGenerator;
    protected readonly IntegrationTestWebAppFactory _factory;

    protected const string DefaultExternalUserEmail = "IntegrationTests.User1@tasksync.test";

    public BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _factory = factory;
        
        var customizedFactory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                // Remove the original implementation and add our mock
                services.RemoveAll<IExternalUserRepository>();
                services.AddSingleton<IExternalUserRepository>(new MockExternalUserRepository());
                services.AddSingleton<TestJwtGenerator>();

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
        });
        
        _client = customizedFactory.CreateClient();
        _testJwtGenerator = customizedFactory.Services.GetRequiredService<TestJwtGenerator>();
        
    }

    /// <summary>
    /// Sets Authorization Header for requests.
    /// </summary>
    /// <param name="roles"></param>
    /// <param name="externalUserId"></param>
    /// <param name="email"></param>
    protected void SetAuthenticatedUser(string[]? roles = null, string externalUserId = "integration_tests|01", string? email = DefaultExternalUserEmail)
    {
        roles ??= ["User"];

        var jwt = _testJwtGenerator.Generate(externalUserId, email, roles);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
    }

    protected async Task AssertEndpointsReturnUnauthorized((string Url, HttpMethod Method, object? Payload)[] endpoints)
    {
        foreach (var (url, method, payload) in endpoints)
        {
            HttpResponseMessage response;
            if (method == HttpMethod.Post)
            {
                response = await _client.PostAsJsonAsync(url, payload);
            }
            else if (method == HttpMethod.Get)
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

    protected List<string> GetErrorDetails(JsonDocument? document)
    {
        var errorList = (document?.RootElement.GetProperty("errorDetails")
            .EnumerateArray()
            .Select(e => e.ToString())
            .ToList()) ?? [];
        return errorList;
    }
}
