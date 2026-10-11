using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TaskSync;

public class ApiKeyAuthFilter : IAuthorizationFilter
{
    public const string ApiKeySectionName = "LocalAuth:ApiKey";
    public const string ApiKeyHeaderName = "X-Api-Key";
    
    private readonly IConfiguration _configuration;
    private readonly ILogger<ApiKeyAuthFilter> _logger;

    public ApiKeyAuthFilter(IConfiguration configuration, ILogger<ApiKeyAuthFilter> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName,
                out var requestApiKey))
        {
            context.Result = new UnauthorizedObjectResult("API Key is missing in request headers");
            return;
        }

        var apiKey = _configuration.GetValue<string>(ApiKeySectionName);
        if (string.IsNullOrWhiteSpace((apiKey)))
        {
            _logger.LogError("No API Key defined in appsettings.");
            context.Result = new ObjectResult("Internal server error")
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
            return;
        }

        if (!apiKey.Equals(requestApiKey))
        {
            context.Result = new UnauthorizedObjectResult("Invalid API Key");
            return;
        }
    }
}