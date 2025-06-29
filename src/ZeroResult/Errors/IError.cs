namespace ZeroResult.Errors;

/// <summary>
/// Represents an error type for Result monads.
/// </summary>
public interface IError
{
    /// <summary>
    /// Gets a descriptive error message.
    /// </summary>
    string Message { get; }

    /// <summary>
    /// Optional error code to identify the error type.
    /// </summary>
    string? Code { get; }
}
