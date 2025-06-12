using BenchmarkDotNet.Attributes;
using ZeroResult.Core.Errors;
using ZeroResult.Core.Models;

namespace ZeroResult.Benchmarks;

[MemoryDiagnoser]
[ShortRunJob]
public class MapBindBenchmarks
{
    private readonly Random _random = new(42);
    private readonly int _successThreshold = 100; // 100% success rate

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
    public int ResultMonadChaining()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            sum += StackResultMethod(_random.Next(100))
                .Map(x => x * 2)
                .Bind(x => x > 10
                    ? StackResult.Success<int, BasicError>(x + 10)
                    : StackResult.Failure<int, BasicError>(new BasicError("Too small")))
                .Match(
                    onSuccess: x => x,
                    onFailure: _ => -1);
        }

        return sum;
    }

    private int TraditionalMethod(int input)
    {
        if (input <= _successThreshold)
        {
            return input;
        }

        throw new InvalidOperationException("Failed");
    }

    private int TraditionalTransform(int input)
    {
        if (input <= 100)
        {
            return input + 10;
        }

        throw new InvalidOperationException("Too small");
    }

    private StackResult<int, BasicError> StackResultMethod(int input)
    {
        if (input <= _successThreshold)
        {
            return StackResult.Success<int, BasicError>(input);
        }

        return StackResult.Failure<int, BasicError>(new BasicError("Failed"));
    }
}

public class BasicError(string message) : IError
{
    public string Message { get; } = message;

    public string? Code => throw new NotImplementedException();
}
