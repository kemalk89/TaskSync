using Microsoft.EntityFrameworkCore;

using TaskSync.Domain.User;
using TaskSync.Infrastructure.Entities;

namespace TaskSync.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DatabaseContext _dbContext;

    public UserRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Task<User[]> SearchUsersAsync(string searchText)
    {
        throw new NotImplementedException();
    }

    public async Task<User[]> FindUsersAsync(int[] userIds)
    {
        if (userIds.Length == 0)
        {
            return [];
        }

        var userRecords = await _dbContext.Users.Where(u => userIds.Contains(u.Id)).ToListAsync(); 
        var users = userRecords.Select(u => new User
        {
            Id = u.Id,
            ExternalUserId = u.ExternalUserId,
            Username = u.Username,
            Email = u.Email,
            Picture = u.Picture
        });

        return users.ToArray();
    }

    public async Task<User?> FindUserByIdAsync(int userId)
    {
        var entity = await _dbContext.Users.FindAsync(userId);

        return entity?.ToUser();
    }

    public async Task<User[]> FindUsersAsync(int pageNumber, int pageSize)
    {
        if (pageNumber <= 0) pageNumber = 1;
        if (pageSize <= 0) pageSize = 10;
        
        var skip = (pageNumber - 1) * pageSize;
        
        var userEntities = await _dbContext.Users
            .OrderBy(u => u.Username)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();
        
        var users = userEntities.Select(u => u.ToUser());

        return users.ToArray();
    }

    public async Task<int> SaveNewUserAsync(User user)
    {
        var entity = new UserEntity
        {
            ExternalUserId = user.ExternalUserId, Email = user.Email, Username = user.Username, Picture = user.Picture,
        };
        await _dbContext.Users.AddAsync(entity);
        
        await _dbContext.SaveChangesAsync();

        return entity.Id;
    }

    public async Task<int> UpdateUserAsync(User user)
    {
        var entity = await _dbContext.Users.FindAsync(user.Id);

        if (entity == null)
        {
            throw new NotImplementedException();
        }
        
        entity.Update(user);
        
        _dbContext.Users.Update(entity);
        await _dbContext.SaveChangesAsync();
        
        return entity.Id;
    }

    public async Task<User?> FindUserByExternalUserIdAsync(string externalUserId)
    {
        var entity = await _dbContext.Users.Where(u => u.ExternalUserId == externalUserId).FirstOrDefaultAsync();
        
        return entity?.ToUser();
    }

    public async Task<User?> FindUserByEmailAsync(string email)
    {
        var entity = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        return entity?.ToUser();
    }
}