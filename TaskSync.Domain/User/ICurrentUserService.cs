using TaskSync.Domain.Shared;

namespace TaskSync.Domain.User;

public interface ICurrentUserService
{
    Task<Result<User>> GetCurrentUserAsync();

    Task<Result<string>> ChangeLanguageAsync(string language);
}