using System.Security.Claims;

using TaskSync.Domain.Shared;

namespace TaskSync.Domain.User;

public interface ICurrentUserService
{
    /// <summary>
    /// Returns authenticated user.
    /// </summary>
    Task<Result<User>> GetCurrentUserAsync();

    /// <summary>
    /// Returns user by provided principal.
    /// </summary>
    Task<Result<User>> GetCurrentUserAsync(ClaimsPrincipal principal);

    Task<Result<string>> ChangeLanguageAsync(string language);
}