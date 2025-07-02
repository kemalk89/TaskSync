namespace TaskSync.Domain.User;

public interface ICurrentUserService
{
    Task<User?> GetCurrentUserAsync();
}