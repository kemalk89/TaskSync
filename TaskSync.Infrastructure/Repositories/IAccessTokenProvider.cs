namespace TaskSync.Infrastructure.Repositories;

public interface IAccessTokenProvider
{
    Task<string?> GetAccessTokenAsync();
}
