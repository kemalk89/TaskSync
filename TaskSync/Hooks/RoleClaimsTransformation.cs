using System.Security.Claims;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

using TaskSync.Domain.User;
using TaskSync.Infrastructure.Entities;

namespace TaskSync.Hooks;

/// <summary>
/// Called by the authentication service only after a successful authentication.
/// </summary>
public class RoleClaimsTransformation : IClaimsTransformation
{
    private readonly ICurrentUserService _currentUserService;
    private readonly UserManager<UserEntity> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public RoleClaimsTransformation(
        UserManager<UserEntity> userManager, 
        IHttpContextAccessor httpContextAccessor, 
        ICurrentUserService currentUserService)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _currentUserService = currentUserService;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        ClaimsIdentity claimsIdentity = new();
        if (!principal.HasClaim(claim => claim.Type == ClaimTypes.Role))
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                var currentUser = await _currentUserService.GetCurrentUserAsync(principal);
                if (currentUser is { Success: true, Value: not null })
                {
                    var roles = await _userManager.GetRolesAsync(new UserEntity
                    {
                        Id = currentUser.Value.Id
                    });
                    
                    foreach (var role in roles)
                    {
                        claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
                    }    
                }
            }
        }

        principal.AddIdentity(claimsIdentity);
        return principal;
    }
}