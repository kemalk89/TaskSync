using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using TaskSync.Infrastructure;

using Testcontainers.PostgreSql;

namespace TaskSync.Tests.IntegrationTests;

/// <summary>
/// Custom WebApplicationFactory to integrate TestContainers.
/// </summary>
public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres:18-bookworm")
        .WithDatabase("tasksync_test")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<DatabaseContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseNpgsql(_postgres.GetConnectionString());
            });
        });
    }

    public Task InitializeAsync()
    {
        Environment.SetEnvironmentVariable("LocalAuth__JwtSecret", "any-value");
        
        return _postgres.StartAsync();
    }
    
    public new Task DisposeAsync()
    {
        return _postgres.StopAsync();
    }
}