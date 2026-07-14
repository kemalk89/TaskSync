using TaskSync.Infrastructure.Repositories;

namespace TaskSync.Tests.IntegrationTests.User;

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
            Email = "Test.Tester@tasksync.test",
            Username = "testuser",
            Picture = "https://example.com/picture.jpg",
            ExternalUserId = externalUserId
        };
        return Task.FromResult<Domain.User.User?>(user);
    }
}