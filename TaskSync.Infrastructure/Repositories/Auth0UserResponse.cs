namespace TaskSync.Infrastructure.Repositories;

public class Auth0UserResponse
{
    public string user_id { get; set; } = null!;
    public string email { get; set; } = null!;
    public string picture { get; set; } = null!;
    public string name { get; set; } = null!;
}
