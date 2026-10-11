namespace TaskSync.Domain.Auth;

public class TokenResult
{
    public string Jwt { get; set; } = string.Empty;
    public long ExpiresIn { get; set; }
}