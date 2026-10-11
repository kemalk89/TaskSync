using FluentValidation;

using Microsoft.Extensions.Logging;

using TaskSync.Domain.Shared;
using TaskSync.Domain.User;

namespace TaskSync.Domain.Auth;

public class SignInCommandHandler : ICommandHandler
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<SignInCommandHandler> _logger;
    private readonly SignInCommandValidator _validator;
    private readonly ITokenService _tokenService;

    public SignInCommandHandler(
        IUserRepository userRepository, 
        ILogger<SignInCommandHandler> logger, 
        SignInCommandValidator validator, 
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _logger = logger;
        _validator = validator;
        _tokenService = tokenService;
    }

    public async Task<Result<SignInResult>> HandleAsync(SignInCommand command)
    {
        var validationResult = await _validator.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            return Result<SignInResult>.Fail(ResultCodes.ResultCodeValidationFailed, validationResult);
        }
        
        // Generic message to prevent lookup
        const string genericFailMsg = "Email or password is not correct";
        var user = await _userRepository.FindUserByEmailAsync(command.Email);
        if (user == null)
        {
            _logger.LogInformation("Could not find user by email.");
            return Result<SignInResult>.Fail(genericFailMsg);
        }

        if (string.IsNullOrWhiteSpace(user.PasswordHash))
        {
            _logger.LogInformation("Tried to login but no password hash exists. User: {UserId}", user.Id);
            return Result<SignInResult>.Fail(genericFailMsg);
        }

        var passwordVerification = await _userRepository.VerifyPasswordAsync(
            user.Email, user.PasswordHash, command.Password);
        if (!passwordVerification.Success)
        {
            return Result<SignInResult>.Fail(genericFailMsg);
        }

        var jwtResult = _tokenService.GenerateJwt(user);
        
        var result = new SignInResult
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.Email,
            AccessToken = jwtResult.Jwt,
            ExpiresIn = jwtResult.ExpiresIn
        };

        return Result<SignInResult>.Ok(result);
    }
}

public record SignInCommand(string Email, string Password);

public class SignInCommandValidator : AbstractValidator<SignInCommand>
{
    public SignInCommandValidator()
    {
        const string msg = "Email or password cannot be empty";
        RuleFor(x => x.Email).NotEmpty().WithMessage(msg);
        RuleFor(x => x.Password).NotEmpty().WithMessage(msg);
    }
}

public class SignInResult
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public long ExpiresIn { get; set; }
}