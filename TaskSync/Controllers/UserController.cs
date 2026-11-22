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
    /// This method checks that users can only sync their own external accounts. For this check, the authenticated user
    /// must have claim "email".
    /// </summary>
    [HttpPost]
    [Route("external")]
    public async Task<ActionResult> SyncExternalUser([FromBody] SyncExternalUserRequest request)
    {
        // Input Validation
        if (string.IsNullOrWhiteSpace(request.ExternalUserEmail) || string.IsNullOrWhiteSpace(request.ExternalUserId))
        {
            return BadRequest("Invalid email or user id");
        }

        var currentUserEmail = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(claim => claim.Type == "email")?.Value;
        if (string.IsNullOrWhiteSpace(currentUserEmail))
        {
            _logger.LogError("Current user has no claim 'email'. Cannot sync external user.");
            return Conflict("No claim 'email' exists associated with authenticated user");
        }
        
        if (currentUserEmail != request.ExternalUserEmail)
        {
            _logger.LogError("Users can only sync their own accounts (current users email is not matching the provided external users email).");
            return BadRequest("Users can only sync their own external accounts");
        }
        
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
            await _userRepository.SaveNewExternalUserAsync(externalUser);
            return StatusCode(201);
        }

        _logger.LogDebug("Updating data of external user with ID: {id}", request.ExternalUserId);

        if (string.IsNullOrWhiteSpace(user.Username))
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
