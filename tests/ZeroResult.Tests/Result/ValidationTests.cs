namespace ZeroResult.Tests;

public class ValidationTests
{
    [Fact]
    public void Ensure_ShouldReturnOriginal_WhenSuccessAndPredicateTrue()
    {
        var result = Result.Success<int, BasicError>(42)
            .Ensure(x => x > 0, () => TestData.EnsureError);

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void Ensure_ShouldReturnError_WhenSuccessAndPredicateFalse()
    {
        var result = Result.Success<int, BasicError>(42)
            .Ensure(x => x < 0, () => TestData.EnsureError);

        Assert.False(result.IsSuccess);
        Assert.Equal(TestData.EnsureError, result.Error);
    }

    [Fact]
    public void Ensure_ShouldReturnOriginalError_WhenFailure()
    {
        var result = Result.Failure<int, BasicError>(TestData.TestError)
            .Ensure(x => x > 0, () => TestData.EnsureError);

        Assert.False(result.IsSuccess);
        Assert.Equal(TestData.TestError, result.Error);
    }

    [Fact]
    public async Task EnsureAsync_ShouldReturnOriginal_WhenSuccessAndPredicateTrue()
    {
        var result = await Result.Success<int, BasicError>(42)
            .EnsureAsync(
                x => ValueTask.FromResult(x > 0),
                () => ValueTask.FromResult(TestData.EnsureError));

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public async Task EnsureAsync_ShouldReturnError_WhenSuccessAndPredicateFalse()
    {
        var result = await Result.Success<int, BasicError>(42)
            .EnsureAsync(
                x => ValueTask.FromResult(x < 0),
                () => ValueTask.FromResult(TestData.EnsureError));

        Assert.False(result.IsSuccess);
        Assert.Equal(TestData.EnsureError, result.Error);
    }
}
