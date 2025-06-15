using BenchmarkDotNet.Attributes;
using ZeroResult.Errors;

namespace ZeroResult.Benchmarks;

[MemoryDiagnoser]
[ShortRunJob]
public class MapBindBenchmarks
{
    private readonly Random _random = new(42);
    
    [Params(0, 50, 100)]
    public int SuccessThreshold { get; set; }

    [Params(100, 1000)]
    public int Iterations { get; set; }

    [Benchmark(Baseline = true)]
    public int TraditionalChaining()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            try
            {
                var value = TraditionalMethod(_random.Next(100));
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
    public int StackResultTraditionalStyle()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var result = StackResultTraditionalMethod(_random.Next(100));
            if (result.IsSuccess)
            {
                var transformed = StackResultTraditionalTransform(result.Value);
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
    public int HeapResultTraditionalStyle()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var result = HeapResultTraditionalMethod(_random.Next(100));
            if (result.IsSuccess)
            {
                var transformed = HeapResultTraditionalTransform(result.Value);
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
    public int StackResultBindChaining()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            sum += StackResultMethod(_random.Next(100))
                .Bind(x => x <= SuccessThreshold
                    ? StackResult.Success<int, BasicError>(x + 10)
                    : StackResult.Failure<int, BasicError>(new BasicError("Too small")))
                .Match(
                    onSuccess: x => x,
                    onFailure: _ => -1);
        }

        return sum;
    }

    [Benchmark]
    public int HeapResultBindChaining()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            sum += HeapResultMethod(_random.Next(100))
                .Bind(x => x <= SuccessThreshold
                    ? HeapResult.Success<int, BasicError>(x + 10)
                    : HeapResult.Failure<int, BasicError>(new BasicError("Too small")))
                .Match(
                    onSuccess: x => x,
                    onFailure: _ => -1);
        }

        return sum;
    }

    [Benchmark]
    public int StackResultMapChaining()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            sum += StackResultMethod(_random.Next(100))
                .Map(x => x <= SuccessThreshold ? x + 10 : -1)
                .Match(
                    onSuccess: x => x,
                    onFailure: _ => -1);
        }

        return sum;
    }

    [Benchmark]
    public int HeapResultMapChaining()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            sum += HeapResultMethod(_random.Next(100))
                .Map(x => x <= SuccessThreshold ? x + 10 : -1)
                .Match(
                    onSuccess: x => x,
                    onFailure: _ => -1);
        }

        return sum;
    }

    private StackResult<int, BasicError> StackResultTraditionalMethod(int input)
    {
        if (input <= SuccessThreshold)
        {
            return StackResult.Success<int, BasicError>(input);
        }

        return StackResult.Failure<int, BasicError>(new BasicError("Failed"));
    }

    private StackResult<int, BasicError> StackResultTraditionalTransform(int input)
    {
        if (input <= SuccessThreshold)
        {
            return StackResult.Success<int, BasicError>(input + 10);
        }

        return StackResult.Failure<int, BasicError>(new BasicError("Too small"));
    }

    private HeapResult<int, BasicError> HeapResultTraditionalMethod(int input)
    {
        if (input <= SuccessThreshold)
        {
            return HeapResult.Success<int, BasicError>(input);
        }

        return HeapResult.Failure<int, BasicError>(new BasicError("Failed"));
    }

    private HeapResult<int, BasicError> HeapResultTraditionalTransform(int input)
    {
        if (input <= SuccessThreshold)
        {
            return HeapResult.Success<int, BasicError>(input + 10);
        }

        return HeapResult.Failure<int, BasicError>(new BasicError("Too small"));
    }

    private int TraditionalMethod(int input)
    {
        if (input <= SuccessThreshold)
        {
            return input;
        }

        throw new InvalidOperationException("Failed");
    }

    private int TraditionalTransform(int input)
    {
        if (input <= SuccessThreshold)
        {
            return input + 10;
        }

        throw new InvalidOperationException("Too small");
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

public class BasicError(string message) : IError
{
    public string Message { get; } = message;

    public string? Code => throw new NotImplementedException();
}
