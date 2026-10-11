using FluentValidation;

using TaskSync.Domain.Shared;
using TaskSync.Domain.User;

namespace TaskSync.Domain.Auth;

public class SignUpCommandHandler : ICommandHandler
{
    private readonly IUserRepository _userRepository;
    private readonly SignUpCommandValidator _validator;
    
    public SignUpCommandHandler(
        IUserRepository userRepository, SignUpCommandValidator validator)
    {
        _userRepository = userRepository;
        _validator = validator;
    }

    public async Task<Result<int>> HandleAsync(SignUpCommand command)
    {
        var validationResult = await _validator.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            return Result<int>.Fail(ResultCodes.ResultCodeValidationFailed, validationResult);
        }
        
        var user = new User.User { Email = command.Email };
        var saveResult = await _userRepository.SaveNewUserAsync(user, command.Password);
        return saveResult.Success 
            ? Result<int>.Ok(saveResult.Value) 
            : Result<int>.Fail(saveResult.Error);
    }
}

public record SignUpCommand(string Email, string Password);

public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
{
    public SignUpCommandValidator(IUserRepository userRepository)
    {
        const int maxLength = 64;
        const string msg = "Email or password cannot be empty";
        RuleFor(x => x.Email).NotEmpty().WithMessage(msg);
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(msg)
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
            .MaximumLength(maxLength).WithMessage($"Password must not exceed {maxLength} characters.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase character.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase character.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.");
        
        // Check if email is already registered
        RuleFor(x => x.Email).MustAsync(async (email, cancellation) =>
        {
            var exists = await userRepository.FindUserByEmailAsync(email);
            return exists == null;
        }).WithMessage("Something went wrong. Please try again.");
    }
}
