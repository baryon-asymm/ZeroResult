namespace ZeroResult.Tests;

public class AsyncOperationsTests
{
    [Fact]
    public async Task ImplicitConversion_ShouldWork_ForValueTask()
    {
        ValueTask<Result<int, BasicError>> task = Result.Success<int, BasicError>(42);
        var result = await task;

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public async Task CombinedAsyncOperations_ShouldWorkCorrectly()
    {
        var result = await Result.Success<int, BasicError>(42)
            .MapAsync(x => ValueTask.FromResult(x.ToString()))
            .BindAsync(s => ValueTask.FromResult(Result.Success<int, BasicError>(s.Length)))
            .EnsureAsync(i => ValueTask.FromResult(i > 0), () => ValueTask.FromResult(TestData.TestError));

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value);
    }

    [Fact]
    public async Task AsyncOperations_ShouldPreserveError_WhenFailure()
    {
        var result = await Result.Failure<int, BasicError>(TestData.TestError)
            .MapAsync(x => ValueTask.FromResult(x.ToString()))
            .BindAsync(s => ValueTask.FromResult(Result.Success<int, BasicError>(s.Length)))
            .OnSuccessAsync(_ => ValueTask.CompletedTask);

        Assert.False(result.IsSuccess);
        Assert.Equal(TestData.TestError, result.Error);
    }
}
