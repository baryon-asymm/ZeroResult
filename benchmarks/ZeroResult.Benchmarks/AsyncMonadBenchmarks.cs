using BenchmarkDotNet.Attributes;
using ZeroResult.Benchmarks.Common;

namespace ZeroResult.Benchmarks;

[MemoryDiagnoser]
[ShortRunJob]
public class AsyncMonadBenchmarks : BenchmarkBase
{
    [Benchmark(Baseline = true)]
    public async Task<int> ExceptionHandlingAsync()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            try
            {
                sum += await TraditionalAsyncMethod(Random.Next(100));
            }
            catch (InvalidOperationException)
            {
                sum -= 1;
            }
        }

        return sum;
    }

    [Benchmark]
    public async ValueTask<int> ResultAsync()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var result = await ResultAsyncMethod(Random.Next(100));
            sum += await result.MatchAsync(
                onSuccess: x => ValueTask.FromResult(x),
                onFailure: _ => ValueTask.FromResult(-1));
        }
        
        return sum;
    }

    private async Task<int> TraditionalAsyncMethod(int input)
    {
        await Task.Delay(BenchmarkConstants.AsyncDelayMs);
        return TraditionalMethod(input);
    }

    private async ValueTask<Result<int, BasicError>> ResultAsyncMethod(int input)
    {
        await Task.Delay(BenchmarkConstants.AsyncDelayMs);
        return ResultMethod(input);
    }
}
