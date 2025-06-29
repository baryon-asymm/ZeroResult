using BenchmarkDotNet.Attributes;

namespace ZeroResult.Benchmarks.Common;

public abstract class BenchmarkBase
{
    protected readonly Random Random = new(BenchmarkConstants.RandomSeed);

    [Params(0, 75, 100)]
    public int SuccessThreshold { get; set; }

    [Params(2000)]
    public int Iterations { get; set; }

    protected int TraditionalMethod(int input)
    {
        return input <= SuccessThreshold
            ? input
            : throw new InvalidOperationException("Failed");
    }

    protected StackResult<int, BasicError> StackResultMethod(int input)
    {
        return input <= SuccessThreshold
            ? StackResult.Success<int, BasicError>(input)
            : StackResult.Failure<int, BasicError>(new BasicError("Failed"));
    }

    protected Result<int, BasicError> ResultMethod(int input)
    {
        return input <= SuccessThreshold
            ? Result.Success<int, BasicError>(input)
            : Result.Failure<int, BasicError>(new BasicError("Failed"));
    }

    protected int ExternalLibraryMethod(int input)
    {
        return input <= SuccessThreshold
            ? input
            : throw new InvalidOperationException("External call failed");
    }
}
