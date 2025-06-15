using BenchmarkDotNet.Attributes;

namespace ZeroResult.Benchmarks;

[MemoryDiagnoser]
[ShortRunJob]
public class MonadBenchmarks
{
    private readonly Random _random = new(42);

    [Params(0, 50, 100)]
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
    public int StackResultMonadApproach()
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

    [Benchmark]
    public int HeapResultMonadApproach()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var result = HeapResultMethod(_random.Next(100));
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

    private StackResult<int, BasicError> StackResultMethod(int input)
    {
        if (input <= SuccessThreshold)
        {
            return StackResult.Success<int, BasicError>(input);
        }

        return StackResult.Failure<int, BasicError>(new BasicError("Failed"));
    }

    private HeapResult<int, BasicError> HeapResultMethod(int input)
    {
        if (input <= SuccessThreshold)
        {
            return HeapResult.Success<int, BasicError>(input);
        }

        return HeapResult.Failure<int, BasicError>(new BasicError("Failed"));
    }
}
