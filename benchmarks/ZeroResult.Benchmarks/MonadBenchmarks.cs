using BenchmarkDotNet.Attributes;
using ZeroResult.Benchmarks.Common;

namespace ZeroResult.Benchmarks;

[MemoryDiagnoser]
[ShortRunJob]
public class MonadBenchmarks : BenchmarkBase
{
    [Benchmark(Baseline = true)]
    public int ExceptionHandling()
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
    public int HeapResultMonad()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var result = HeapResultMethod(Random.Next(100));
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
    public int HeapResult_ExceptionToMonad()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var result = CallExternalLib_WithHeapResult(Random.Next(100));
            sum += result.Match(
                onSuccess: x => x,
                onFailure: _ => -1);
        }

        return sum;
    }

    private StackResult<int, BasicError> CallExternalLib_WithStackResult(int input)
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

    private HeapResult<int, BasicError> CallExternalLib_WithHeapResult(int input)
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
