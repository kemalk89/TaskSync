using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using TaskSync.Controllers.Request;
using TaskSync.Controllers.Response;
using TaskSync.Domain.User;
using TaskSync.Domain.Shared;
using TaskSync.Infrastructure.Repositories;

namespace TaskSync.Controllers;

[Authorize]
[ApiController]
[Route("/api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;
    private readonly IExternalUserRepository _externalUserRepository;
    private readonly ILogger<UserController> _logger;

    public UserController(
        IUserService userService, 
        IUserRepository userRepository,
        IExternalUserRepository externalUserRepository, 
        ILogger<UserController> logger)
    {
        _userService = userService;
        _userRepository = userRepository;
        _externalUserRepository = externalUserRepository;
        _logger = logger;
    }

    /// <summary>
    /// Will fetch external user and insert into this application's database. This endpoint should be called,
    /// for example, after a successful user login.
    /// </summary>
    [HttpPost]
    [Route("external")]
    public async Task<ActionResult<string>> SyncExternalUser([FromBody] SyncExternalUserRequest request)
    {
        var externalUser = await _externalUserRepository.FindUserByIdFromExternalSourceAsync(request.ExternalUserId);
        if (externalUser == null)
        {
            _logger.LogWarning("External data source returned no user for ID: {id}", request.ExternalUserId);
            return NotFound();
        }

        var user = await _userRepository.FindUserByEmailAsync(externalUser.Email);
        if (user == null)
        {
            _logger.LogInformation("Adding new external user with ID: {id}", request.ExternalUserId);
            await _userRepository.SaveNewUserAsync(externalUser);
            return Created();
        }
        
        _logger.LogDebug("Updating data of external user with ID: {id}", request.ExternalUserId);

        if (user.Picture.IsNullOrEmpty())
        {
            user.Picture = externalUser.Picture;
        }
        if (user.Username.IsNullOrEmpty())
        {
            user.Username = externalUser.Username;
        }
        
        user.ExternalUserId = request.ExternalUserId;
        await _userRepository.UpdateUserAsync(user);

        return Ok();
    }
    
    [HttpPost]
    public async Task<IEnumerable<UserResponse>> SearchUsersAsync([FromBody] SearchUserRequest req)
    {
        var users = await _userService.SearchUserAsync(req.ToCommand());
        return users.Select(item => new UserResponse(item));
    }

    [HttpGet]
    public async Task<PagedResult<UserResponse>> GetUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50)
    {
        PagedResult<User> pagedResult = await _userService.GetUsersAsync(pageNumber, pageSize);

        return new PagedResult<UserResponse>
        {
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize,
            Items = pagedResult.Items.Select(item => new UserResponse(item)),
            Total = pagedResult.Total
        };
    }
}
