namespace TaskSync.Domain.Shared;

/// <summary>
/// Reusable result codes for the <see cref="Result{T}"/> "Error" property.
/// </summary>
public class ResultCodes
{
    public const string ResultCodeResourceNotFound = "RESOURCE_NOT_FOUND";
    public const string ResultCodeNoPermissions = "NO_PERMISSIONS_FOR_THE_OPERATION";
    public const string ResultCodeValidationFailed = "VALIDATION_FAILED";
    public const string ResultCodeGeneralProblem = "GENERAL_PROBLEM";
}