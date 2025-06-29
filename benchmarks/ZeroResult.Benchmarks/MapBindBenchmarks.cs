using BenchmarkDotNet.Attributes;
using ZeroResult.Benchmarks.Common;

namespace ZeroResult.Benchmarks;

[MemoryDiagnoser]
[RankColumn]
public class MapBindBenchmarks : BenchmarkBase
{
    [Benchmark(Baseline = true)]
    public int TryCatchChaining()
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
    public int Result_ImperativeStyle()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var result = ResultMethod(Random.Next(100));
            if (result.IsSuccess)
            {
                var transformed = ResultTransform(result.Value);
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
    public int Result_ImperativeStyle_ExceptionToMonad()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var result = CallExternalLib_WithResult(Random.Next(100));
            if (result.IsSuccess)
            {
                var transformed = ResultTransform(result.Value);
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
    public int Result_Bind()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            sum += ResultMethod(Random.Next(100))
                .Bind(TransformValue)
                .Match(
                    onSuccess: x => x,
                    onFailure: _ => -1);
        }

        return sum;
    }

    [Benchmark]
    public int Result_Bind_ExceptionToMonad()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            sum += CallExternalLib_WithResult(Random.Next(100))
                .Bind(TransformValue)
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
    public int Result_Map()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            sum += ResultMethod(Random.Next(100))
                .Map(TransformDirectly)
                .Match(
                    onSuccess: x => x,
                    onFailure: _ => -1);
        }

        return sum;
    }

    [Benchmark]
    public int Result_Map_ExceptionToMonad()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            sum += CallExternalLib_WithResult(Random.Next(100))
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

    private Result<int, BasicError> ResultTransform(int input)
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

    private Result<int, BasicError> TransformValue(int input)
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

    private int TransformDirectly(int input) => input <= SuccessThreshold ? input + 10 : -1;
}
