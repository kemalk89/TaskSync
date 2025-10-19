namespace TaskSync.Controllers.Response;

/// <summary>
/// An error response which can be returned by controllers. For example in case of Bad Requests.
/// </summary>
public class ErrorResponse
{
    public string ErrorCode { get; set; }
    
    public string[]  ErrorDetails { get; set; }

    public ErrorResponse(string errorCode, string[] errorDetails)
    {
        ErrorCode = errorCode;
        ErrorDetails = errorDetails;
    }
}