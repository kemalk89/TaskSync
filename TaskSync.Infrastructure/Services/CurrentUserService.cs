using Microsoft.AspNetCore.Http;

using TaskSync.Domain.User;

namespace TaskSync.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor, IUserService userService)
    {
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
    }
    
    public async Task<User?> GetCurrentUserAsync()
    {
        var externalUserId = _httpContextAccessor.HttpContext?.User.Identity?.Name;
        var currentUser = await _userService.FindByExternalUserIdAsync(externalUserId);
        return currentUser;
    }
}