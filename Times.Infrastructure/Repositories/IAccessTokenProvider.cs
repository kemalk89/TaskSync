namespace Times.Infrastructure.Repositories;

public interface IAccessTokenProvider
{
    Task<string?> GetAccessTokenAsync();
}
