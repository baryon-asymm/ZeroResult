using BenchmarkDotNet.Attributes;
using ZeroResult.Benchmarks.Common;

namespace ZeroResult.Benchmarks;

[MemoryDiagnoser]
[RankColumn]
public class MonadBenchmarks : BenchmarkBase
{
    [Benchmark(Baseline = true)]
    public int TryCatchHandling()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            try
            {
                sum += TraditionalMethod(Random.Next(100));
            }
            catch (InvalidOperationException)
            {
                sum -= 1;
            }
        }

        return sum;
    }

    [Benchmark]
    public int StackResultMonad()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var result = StackResultMethod(Random.Next(100));
            sum += result.Match(
                onSuccess: x => x,
                onFailure: _ => -1);
        }

        return sum;
    }

    [Benchmark]
    public int ResultMonad()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var result = ResultMethod(Random.Next(100));
            sum += result.Match(
                onSuccess: x => x,
                onFailure: _ => -1);
        }

        return sum;
    }

    [Benchmark]
    public int StackResult_ExceptionToMonad()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var result = CallExternalLib_WithStackResult(Random.Next(100));
            sum += result.Match(
                onSuccess: x => x,
                onFailure: _ => -1);
        }

        return sum;
    }

    [Benchmark]
    public int Result_ExceptionToMonad()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var result = CallExternalLib_WithResult(Random.Next(100));
            sum += result.Match(
                onSuccess: x => x,
                onFailure: _ => -1);
        }

        return sum;
    }

    private Result<int, BasicError> CallExternalLib_WithStackResult(int input)
    {
        try
        {
            return ExternalLibraryMethod(input);
        }
        catch (Exception ex)
        {
            return new BasicError(ex.Message);
        }
    }

    private Result<int, BasicError> CallExternalLib_WithResult(int input)
    {
        try
        {
            return ExternalLibraryMethod(input);
        }
        catch (Exception ex)
        {
            return new BasicError(ex.Message);
        }
    }
}
