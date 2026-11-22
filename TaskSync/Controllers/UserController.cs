using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly IExternalUserRepository _externalUserRepository;
    private readonly ILogger<UserController> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserController(
        IUserService userService,
        IUserRepository userRepository,
        IExternalUserRepository externalUserRepository,
        ICurrentUserService currentUserService,
        ILogger<UserController> logger, 
        IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _userRepository = userRepository;
        _externalUserRepository = externalUserRepository;
        _currentUserService = currentUserService;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Will fetch external user and insert into this application's database. This endpoint should be called,
    /// for example, after a successful user login.
    /// </summary>
    [HttpPost]
    [Route("external")]
    public async Task<ActionResult> SyncExternalUser()
    {
        var currentUserNameIdentifier = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(currentUserNameIdentifier))
        {
            _logger.LogError("Current user has no claim '{NameIdentifier}'. Cannot sync external user.", ClaimTypes.NameIdentifier);
            return Conflict("No claim '" + ClaimTypes.NameIdentifier + "' exists associated with authenticated user");
        }
        
        var userFromExternalIdP = await _externalUserRepository.FindUserByIdFromExternalSourceAsync(currentUserNameIdentifier);
        if (userFromExternalIdP == null)
        {
            _logger.LogWarning("Unable to find current user in external data source");
            return NotFound("Unable to find current user in external data source");
        }
        
        var user = await _userRepository.FindUserByEmailAsync(userFromExternalIdP.Email);
        if (user == null)
        {
            _logger.LogInformation("Adding new external user with ID: {id}", currentUserNameIdentifier);
            await _userRepository.SaveNewExternalUserAsync(userFromExternalIdP);
            return StatusCode(201);
        }

        _logger.LogDebug("Updating data of external user with ID: {id}", currentUserNameIdentifier);

        if (string.IsNullOrWhiteSpace(user.Username))
        {
            user.Username = userFromExternalIdP.Username;
        }

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

    [HttpPatch]
    [Route("changeLanguage/{language}")]
    public async Task<ActionResult<string>> ChangeUserLanguage([FromRoute] string language)
    {
        var result = await _currentUserService.ChangeLanguageAsync(language);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result.Value);
    }

    [HttpGet]
    [Route("current-user")]
    public async Task<ActionResult<User>> GetCurrentUser()
    {
        var result = await _currentUserService.GetCurrentUserAsync();

        return Ok(result);
    }
}
