namespace TaskSync.Controllers.Response;

/// <summary>
/// An error response which can be returned by controllers. For example in case of Bad Requests.
/// </summary>
public class ErrorResponse
{
    public string[]  Errors { get; set; }

    public ErrorResponse(string[] errors)
    {
        Errors = errors;
    }
}