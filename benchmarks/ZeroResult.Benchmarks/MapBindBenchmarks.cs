using BenchmarkDotNet.Attributes;
using ZeroResult.Benchmarks.Common;

namespace ZeroResult.Benchmarks;

[MemoryDiagnoser]
[ShortRunJob]
public class MapBindBenchmarks : BenchmarkBase
{
    [Benchmark(Baseline = true)]
    public int ExceptionHandlingChaining()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            try
            {
                var value = TraditionalMethod(Random.Next(100));
                var transformed = TraditionalTransform(value);
                sum += transformed;
            }
            catch (InvalidOperationException)
            {
                sum -= 1;
            }
        }

        return sum;
    }

    [Benchmark]
    public int StackResult_ImperativeStyle()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var result = StackResultMethod(Random.Next(100));
            if (result.IsSuccess)
            {
                var transformed = StackResultTransform(result.Value);
                sum += transformed.IsSuccess ? transformed.Value : -1;
            }
            else
            {
                sum -= 1;
            }
        }

        return sum;
    }

    [Benchmark]
    public int StackResult_ImperativeStyle_ExceptionToMonad()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var result = CallExternalLib_WithStackResult(Random.Next(100));
            if (result.IsSuccess)
            {
                var transformed = StackResultTransform(result.Value);
                sum += transformed.IsSuccess ? transformed.Value : -1;
            }
            else
            {
                sum -= 1;
            }
        }

        return sum;
    }

    [Benchmark]
    public int HeapResult_ImperativeStyle()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var result = HeapResultMethod(Random.Next(100));
            if (result.IsSuccess)
            {
                var transformed = HeapResultTransform(result.Value);
                sum += transformed.IsSuccess ? transformed.Value : -1;
            }
            else
            {
                sum -= 1;
            }
        }

        return sum;
    }

    [Benchmark]
    public int HeapResult_ImperativeStyle_ExceptionToMonad()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var result = CallExternalLib_WithHeapResult(Random.Next(100));
            if (result.IsSuccess)
            {
                var transformed = HeapResultTransform(result.Value);
                sum += transformed.IsSuccess ? transformed.Value : -1;
            }
            else
            {
                sum -= 1;
            }
        }

        return sum;
    }

    [Benchmark]
    public int StackResult_Bind()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            sum += StackResultMethod(Random.Next(100))
                .Bind(StackTransformValue)
                .Match(
                    onSuccess: x => x,
                    onFailure: _ => -1);
        }

        return sum;
    }

    [Benchmark]
    public int StackResult_Bind_ExceptionToMonad()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            sum += CallExternalLib_WithStackResult(Random.Next(100))
                .Bind(StackTransformValue)
                .Match(
                    onSuccess: x => x,
                    onFailure: _ => -1);
        }

        return sum;
    }

    [Benchmark]
    public int HeapResult_Bind()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            sum += HeapResultMethod(Random.Next(100))
                .Bind(HeapTransformValue)
                .Match(
                    onSuccess: x => x,
                    onFailure: _ => -1);
        }

        return sum;
    }

    [Benchmark]
    public int HeapResult_Bind_ExceptionToMonad()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            sum += CallExternalLib_WithHeapResult(Random.Next(100))
                .Bind(HeapTransformValue)
                .Match(
                    onSuccess: x => x,
                    onFailure: _ => -1);
        }

        return sum;
    }

    [Benchmark]
    public int StackResult_Map()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            sum += StackResultMethod(Random.Next(100))
                .Map(TransformDirectly)
                .Match(
                    onSuccess: x => x,
                    onFailure: _ => -1);
        }

        return sum;
    }

    [Benchmark]
    public int StackResult_Map_ExceptionToMonad()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            sum += CallExternalLib_WithStackResult(Random.Next(100))
                .Map(TransformDirectly)
                .Match(
                    onSuccess: x => x,
                    onFailure: _ => -1);
        }

        return sum;
    }

    [Benchmark]
    public int HeapResult_Map()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            sum += HeapResultMethod(Random.Next(100))
                .Map(TransformDirectly)
                .Match(
                    onSuccess: x => x,
                    onFailure: _ => -1);
        }

        return sum;
    }

    [Benchmark]
    public int HeapResult_Map_ExceptionToMonad()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            sum += CallExternalLib_WithHeapResult(Random.Next(100))
                .Map(TransformDirectly)
                .Match(
                    onSuccess: x => x,
                    onFailure: _ => -1);
        }

        return sum;
    }

    private int TraditionalTransform(int input)
    {
        return input <= SuccessThreshold
            ? input + 10
            : throw new InvalidOperationException("Too small");
    }

    private StackResult<int, BasicError> StackResultTransform(int input)
    {
        return input <= SuccessThreshold
            ? input + 10
            : new BasicError("Too small");
    }

    private HeapResult<int, BasicError> HeapResultTransform(int input)
    {
        return input <= SuccessThreshold
            ? input + 10
            : new BasicError("Too small");
    }

    private StackResult<int, BasicError> StackTransformValue(int input)
    {
        return input <= SuccessThreshold
            ? input + 10
            : new BasicError("Too small");
    }

    private HeapResult<int, BasicError> HeapTransformValue(int input)
    {
        return input <= SuccessThreshold
            ? input + 10
            : new BasicError("Too small");
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

    private int TransformDirectly(int input) => input <= SuccessThreshold ? input + 10 : -1;
}
