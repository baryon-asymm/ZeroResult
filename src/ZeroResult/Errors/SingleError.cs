namespace ZeroResult.Errors;

/// <summary>
/// Represents a single error with an optional code.
/// This record implements the <see cref="IError"/> interface.
/// The <see cref="Code"/> property can be used to categorize or identify the error,
/// while the <see cref="Message"/> property provides a human-readable description.
/// </summary>
public record SingleError : IError
{
    /// <summary>
    /// Gets the error code, if any.
    /// This can be used to categorize or identify the error.
    /// </summary>
    /// <remarks>
    /// If no code is provided, this will be <c>null</c>.
    /// This is useful for scenarios where you want to provide additional context or categorization for the error.
    /// </remarks>
    /// <value>
    /// The error code as a string, or <c>null</c> if no code is specified.
    /// </value>
    /// <example>
    /// <code>
    /// var error = new SingleError("Invalid input", "VALID_001");
    /// Console.WriteLine(error.Code); // Output: "VALID_001"
    /// </code>
    /// </example>
    public string? Code { get; }

    /// <summary>
    /// Gets the error message.
    /// This provides a human-readable description of the error.
    /// </summary>
    /// <value>
    /// The error message as a string.
    /// </value>
    public string Message { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SingleError"/> record with a specified message and an optional code.
    /// </summary>
    /// <param name="message">Required error description.</param>
    /// <param name="code">Optional categorization code.</param>
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="message"/> is null.
    /// </exception>
    public SingleError(string message, string? code = null)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
        Code = code;
    }

    /// <summary>
    /// Formats the error as "[Code] Message" or "Message" if code is absent.
    /// </summary>
    public override string ToString() => Code is null ? Message : $"[{Code}] {Message}";
}
