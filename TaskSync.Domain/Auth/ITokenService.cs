namespace TaskSync.Domain.Auth;

public interface ITokenService
{
    public TokenResult GenerateJwt(User.User user);
    public bool VerifyJwt(string jwt);
}