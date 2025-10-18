using FluentValidation.Results;

namespace TaskSync.Domain.Shared;

/// <summary>
/// Represents the outcome of an operation, encapsulating either a successful result with a value
/// or a failure with an error message.
/// </summary>
/// <typeparam name="T">The type of the value returned when the operation succeeds.</typeparam>
public class Result<T>
{
    public bool Success { get; }
    public T? Value { get; }
    public string Error { get; }

    public string[] ErrorDetails { get; init; } = [];
    
    /// <summary>
    /// Optional structured information about the error.
    /// For example, for validation failures, this can include field-specific messages.
    /// </summary>
    public Dictionary<string, string>? ErrorPayload { get; init; }

    private Result(bool success, T? value, string? error)
    {
        Success = success;
        Value = value;
        Error = error ?? string.Empty;
    }
    
    public static Result<T> Ok(T value) => new (true, value, null);

    public static Result<T> Fail(string error) => new(false, default, error);
    
    public static Result<T> Fail(string error, Dictionary<string, string>? errorPayload = null) 
        => new (false, default, error)
        {
            ErrorPayload = errorPayload
        };
    
    public static Result<T> Fail(string error, string errorDetails = "", Dictionary<string, string>? errorPayload = null) 
        => new (false, default, error)
        {
            ErrorDetails = [errorDetails],
            ErrorPayload = errorPayload
        };

    public static Result<T> Fail(string error, ValidationResult validationResult)
    {
        var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray();
        return new Result<T>(false, default, error) { ErrorDetails = errors };
    }
}