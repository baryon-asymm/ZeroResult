using ZeroResult.Errors;

namespace ZeroResult.Benchmarks.Common;

public record BasicError(string Message) : IError
{
    public string? Code => null;
}

public static class BenchmarkConstants
{
    public const int AsyncDelayMs = 10;
    public const int RandomSeed = 42;
}
