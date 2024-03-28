using Microsoft.Extensions.DependencyInjection;
using TaskSync.Domain.User;
using TaskSync.Infrastructure.Repositories;

namespace TaskSync.Auth.Auth0;

public static class Auth0ServiceCollectionExtensions
{
    public static IServiceCollection AddAuth0(this IServiceCollection services)
    {
        services.AddScoped<IAccessTokenProvider, Auth0AccessTokenProvider>();
        services.AddScoped<IUserRepository, Auth0UserRepository>();

        return services;
    }
}
