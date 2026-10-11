using Microsoft.AspNetCore.Mvc;

using TaskSync.Domain.Auth;

namespace TaskSync.Controllers.Auth;

[ApiController]
[ServiceFilter(typeof(ApiKeyAuthFilter))]
[Route("/api/v1/[controller]/[action]")]
public class AuthController : ControllerBase
{
    // signOut
    // refresh-token
    // password-reset
    
    private readonly SignInCommandHandler _signInCommandHandler;
    private readonly SignUpCommandHandler _signUpCommandHandler;

    public AuthController(SignInCommandHandler signInCommandHandler, SignUpCommandHandler signUpCommandHandler)
    {
        _signInCommandHandler = signInCommandHandler;
        _signUpCommandHandler = signUpCommandHandler;
    }

    [HttpPost]
    public async Task<ActionResult> SignIn([FromBody] SignInCommand cmd)
    {
        var result = await _signInCommandHandler.HandleAsync(cmd);
        if (!result.Success)
        {
            return Unauthorized("Email or password is not correct");
        }

        var signIn = result.Value!;
        return Ok(new
        {
            signIn.Id,
            signIn.Username,
            signIn.Email,
            access_token = signIn.AccessToken,
            expires_in = signIn.ExpiresIn
        });
    }
    
    [HttpPost]
    public async Task<ActionResult> SignUp([FromBody] SignUpCommand cmd)
    {
        var result = await _signUpCommandHandler.HandleAsync(cmd);
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result.Value);
    }
}
