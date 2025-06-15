using BenchmarkDotNet.Attributes;
using ZeroResult.Core.Models;

namespace ZeroResult.Benchmarks;

[MemoryDiagnoser]
[ShortRunJob]
public class ResultBenchmarks
{
    private readonly Random _random = new(42);

    [Params(0, 25, 50, 75, 100)]
    public int SuccessThreshold { get; set; }

    [Params(100, 1000)]
    public int Iterations { get; set; }

    [Benchmark(Baseline = true)]
    public int TraditionalApproach()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            try
            {
                sum += TraditionalMethod(_random.Next(100));
            }
            catch (InvalidOperationException)
            {
                sum -= 1;
            }
        }

        return sum;
    }

    [Benchmark]
    public int ResultMonadApproach()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var result = ResultMethod(_random.Next(100));
            sum += result.Match(
                onSuccess: x => x,
                onFailure: _ => -1);
        }

        return sum;
    }

    [Benchmark]
    public int ResultMonadApproach_NoAllocations()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var result = StackResultMethod(_random.Next(100));
            sum += result.Match(
                onSuccess: x => x,
                onFailure: _ => -1);
        }

        return sum;
    }

    private int TraditionalMethod(int input)
    {
        if (input <= SuccessThreshold)
        {
            return input;
        }

        throw new InvalidOperationException("Failed");
    }

    private HeapResult<int, BasicError> ResultMethod(int input)
    {
        if (input > SuccessThreshold)
        {
            return HeapResult.Success<int, BasicError>(input);
        }

        return HeapResult.Failure<int, BasicError>(new BasicError("Failed"));
    }

    private StackResult<int, BasicError> StackResultMethod(int input)
    {
        if (input <= SuccessThreshold)
        {
            return StackResult.Success<int, BasicError>(input);
        }

        return StackResult.Failure<int, BasicError>(new BasicError("Failed"));
    }
}
