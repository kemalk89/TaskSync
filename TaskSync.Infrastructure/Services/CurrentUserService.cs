using System.Security.Claims;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using TaskSync.Domain.Shared;
using TaskSync.Domain.User;

namespace TaskSync.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _config;
    private readonly ILogger<CurrentUserService> _logger;
    
    public CurrentUserService(
        IHttpContextAccessor httpContextAccessor, 
        IUserService userService, 
        IUserRepository userRepository, 
        IConfiguration config,
        ILogger<CurrentUserService> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
        _userRepository = userRepository;
        _config = config;
        _logger = logger;
    }
    
    public async Task<Result<User>> GetCurrentUserAsync()
    {
        User? currentUser;
        var issuer = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(claim => claim.Type == "iss");
        var issuerLocalAuth = _config["LocalAuth:ValidIssuer"];
        if (!string.IsNullOrWhiteSpace(issuerLocalAuth) && issuerLocalAuth == issuer?.Value)
        {
            var email = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);
            if (string.IsNullOrWhiteSpace(email?.Value))
            {
                return Result<User>.Fail(ResultCodes.ResultCodeGeneralProblem, "Claim 'email' not found");
            }

            currentUser = await _userRepository.FindUserByEmailAsync(email.Value);
        }
        else
        {
            var externalUserId = _httpContextAccessor.HttpContext?.User.Identity?.Name;
            currentUser = await _userService.FindByExternalUserIdAsync(externalUserId);
        }

        if (currentUser == null)
        {
            return Result<User>.Fail(ResultCodes.ResultCodeResourceNotFound, "Current user not found");
        }
        
        return Result<User>.Ok(currentUser);  
    }
    
    public async Task<Result<string>> ChangeLanguageAsync(string language)
    {
        if (string.IsNullOrWhiteSpace(language))
        {
            return Result<string>.Fail(ResultCodes.ResultCodeValidationFailed, "Invalid language given");
        }

        var currentUser = await GetCurrentUserAsync();

        var user = await _userRepository.FindUserByIdAsync(currentUser.Value!.Id);
        if (user == null)
        {
            return Result<string>.Fail(ResultCodes.ResultCodeResourceNotFound, "No current user found");
        }

        var oldLanguage = user.SelectedLanguage;
        
        user.SelectedLanguage = language;
        var userId = await _userRepository.UpdateUserAsync(user);
        if (userId == 0)
        {
            _logger.LogWarning("Unable to change users language from '{OldLanguage}' to '{Language}'", oldLanguage, language);
            return Result<string>.Fail(
                ResultCodes.ResultCodeGeneralProblem, "Failed to update current users language");
        }
        
        _logger.LogInformation("Changed users language from '{OldLanguage}' to '{Language}'", oldLanguage, language);
        return Result<string>.Ok(language);
    }
}