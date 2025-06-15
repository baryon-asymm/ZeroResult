using BenchmarkDotNet.Attributes;

namespace ZeroResult.Benchmarks;

[MemoryDiagnoser]
[ShortRunJob]
public class AsyncMonadBenchmarks
{
    private readonly Random _random = new(42);
    private const int AsyncDelayMs = 10;

    [Params(0, 50, 100)]
    public int SuccessThreshold { get; set; }

    [Params(100)]
    public int Iterations { get; set; }

    [Benchmark(Baseline = true)]
    public async Task<int> TraditionalAsyncApproach()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            try
            {
                sum += await TraditionalAsyncMethod(_random.Next(100));
            }
            catch (InvalidOperationException)
            {
                sum -= 1;
            }
        }
        return sum;
    }

    [Benchmark]
    public async ValueTask<int> HeapResultAsyncApproach()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var result = await HeapResultAsyncMethod(_random.Next(100));
            sum += await result.MatchAsync(
                onSuccess: x => ValueTask.FromResult(x),
                onFailure: _ => ValueTask.FromResult(-1));
        }
        return sum;
    }

    private async Task<int> TraditionalAsyncMethod(int input)
    {
        await Task.Delay(AsyncDelayMs);

        if (input <= SuccessThreshold)
        {
            return input;
        }

        throw new InvalidOperationException("Failed");
    }

    private async ValueTask<HeapResult<int, BasicError>> HeapResultAsyncMethod(int input)
    {
        await Task.Delay(AsyncDelayMs);

        if (input <= SuccessThreshold)
        {
            return HeapResult.Success<int, BasicError>(input);
        }

        return HeapResult.Failure<int, BasicError>(new BasicError("Failed"));
    }
}
