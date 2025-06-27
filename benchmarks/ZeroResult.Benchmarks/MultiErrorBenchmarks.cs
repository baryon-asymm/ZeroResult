using BenchmarkDotNet.Attributes;
using ZeroResult.Errors;

namespace ZeroResult.Benchmarks;

[MemoryDiagnoser]
[RankColumn]
public class MultiErrorBenchmarks
{
    [Params(20, 200)]
    public int CallDepth { get; set; }

    [Params(1000)]
    public int Iterations { get; set; }

    private readonly Random _random = new(42);

    [Benchmark(Baseline = true)]
    public void ExceptionBasedFlow()
    {
        for (int i = 0; i < Iterations; i++)
        {
            try
            {
                ExceptionBasedMethod(CallDepth, _random.Next());
            }
            catch (Exception) { }
        }
    }

    [Benchmark]
    public void MultiErrorBasedFlow()
    {
        for (int i = 0; i < Iterations; i++)
        {
            var result = OptimizedBuilderMethod(CallDepth, _random.Next());
        }
    }

    [Benchmark]
    public void ListBasedErrorFlow()
    {
        for (int i = 0; i < Iterations; i++)
        {
            var result = ListBasedMethod(CallDepth, _random.Next());
        }
    }

    private void ExceptionBasedMethod(int depth, int randomSeed)
    {
        if (depth <= 0)
            throw new InvalidOperationException($"Base error {randomSeed}");

        try
        {
            ExceptionBasedMethod(depth - 1, randomSeed + depth);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Layer {depth} error {randomSeed}", ex);
        }
    }

    private MultiError MultiErrorMethod(int depth, int randomSeed)
    {
        if (depth <= 0)
            return new MultiError([new TestError($"Base error {randomSeed}", $"BASE_{randomSeed}")]);

        var innerError = MultiErrorMethod(depth - 1, randomSeed + depth);
        var newError = new TestError($"Layer {depth} error {randomSeed}", $"CODE_{randomSeed}");

        var builder = MultiError.CreateBuilder(innerError.Count + 1);
        builder.Add(newError);

        for (int i = 0; i < innerError.Count; i++)
        {
            builder.Add(innerError.GetErrorAt(i));
        }

        return builder.Build();
    }

    private MultiError BuilderBasedMethod(int depth, int randomSeed)
    {
        if (depth <= 0)
        {
            var builderBase = MultiError.CreateBuilder();
            builderBase.Add(new TestError($"Base error {randomSeed}", $"BASE_{randomSeed}"));
            return builderBase.Build();
        }

        var builder = MultiError.CreateBuilder();
        builder.Add(new TestError($"Layer {depth} error {randomSeed}", $"CODE_{randomSeed}"));

        var innerError = BuilderBasedMethod(depth - 1, randomSeed + depth);
        for (int i = 0; i < innerError.Count; i++)
        {
            builder.Add(innerError.GetErrorAt(i));
        }

        return builder.Build();
    }

    private MultiError OptimizedBuilderMethod(int depth, int randomSeed)
    {
        var builder = MultiError.CreateBuilder(initialCapacity: depth + 1);
        BuildErrorsRecursive(in builder, depth, randomSeed);
        return builder.Build();
    }

    private void BuildErrorsRecursive(in MultiErrorBuilder builder, int depth, int randomSeed)
    {
        builder.Add(new TestError(
            depth == 0 ? $"Base error {randomSeed}" : $"Layer {depth} error {randomSeed}",
            depth == 0 ? $"BASE_{randomSeed}" : $"CODE_{randomSeed}"));

        if (depth > 0)
        {
            BuildErrorsRecursive(builder, depth - 1, randomSeed + depth);
        }
    }

    private List<IError> ListBasedMethod(int depth, int randomSeed)
    {
        if (depth <= 0)
            return new List<IError> { new TestError($"Base error {randomSeed}", $"BASE_{randomSeed}") };

        var innerErrors = ListBasedMethod(depth - 1, randomSeed + depth);
        innerErrors.Insert(0, new TestError($"Layer {depth} error {randomSeed}", $"CODE_{randomSeed}"));
        return innerErrors;
    }

    private record TestError(string Message, string Code) : IError;
}
