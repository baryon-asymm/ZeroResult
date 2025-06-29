using ZeroResult.Errors;

namespace ZeroResult.Tests;

public class BasicError : IError
{
    public BasicError(string message, string? code = null)
    {
        Message = message;
        Code = code;
    }

    public string Message { get; }
    public string? Code { get; }

    public override bool Equals(object? obj)
    {
        return obj is BasicError error &&
                Message == error.Message &&
                Code == error.Code;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Message, Code);
    }
}

public static class TestData
{
    public static readonly BasicError TestError = new("TestError");
    public static readonly BasicError EnsureError = new("EnsureError");
}
