namespace ZeroResult.Tests;

public class MappingTests
{
    [Fact]
    public void Map_ShouldTransformValue_WhenSuccess()
    {
        var result = Result.Success<int, BasicError>(42);
        var mapped = result.Map(x => x.ToString());

        Assert.True(mapped.IsSuccess);
        Assert.Equal("42", mapped.Value);
    }

    [Fact]
    public void Map_ShouldReturnOriginalError_WhenFailure()
    {
        var result = Result.Failure<int, BasicError>(TestData.TestError);
        var mapped = result.Map(x => x.ToString());

        Assert.False(mapped.IsSuccess);
        Assert.Equal(TestData.TestError, mapped.Error);
    }

    [Fact]
    public async Task MapAsync_ShouldTransformValue_WhenSuccess()
    {
        var result = Result.Success<int, BasicError>(42);
        var mapped = await result.MapAsync(x => ValueTask.FromResult(x.ToString()));

        Assert.True(mapped.IsSuccess);
        Assert.Equal("42", mapped.Value);
    }

    [Fact]
    public void Bind_ShouldTransformToNewResult_WhenSuccess()
    {
        var result = Result.Success<int, BasicError>(42);
        var bound = result.Bind(x => Result.Success<string, BasicError>(x.ToString()));

        Assert.True(bound.IsSuccess);
        Assert.Equal("42", bound.Value);
    }

    [Fact]
    public void Bind_ShouldReturnOriginalError_WhenFailure()
    {
        var result = Result.Failure<int, BasicError>(TestData.TestError);
        var bound = result.Bind(x => Result.Success<string, BasicError>(x.ToString()));

        Assert.False(bound.IsSuccess);
        Assert.Equal(TestData.TestError, bound.Error);
    }

    [Fact]
    public async Task BindAsync_ShouldTransformToNewResult_WhenSuccess()
    {
        var result = Result.Success<int, BasicError>(42);
        var bound = await result.BindAsync(x =>
            ValueTask.FromResult(Result.Success<string, BasicError>(x.ToString())));

        Assert.True(bound.IsSuccess);
        Assert.Equal("42", bound.Value);
    }

#if RELEASE

    [Fact]
    public void Map_ShouldThrow_WhenMapperIsNull()
    {
        var result = Result.Success<int, BasicError>(42);
        Assert.ThrowsAsync<NullReferenceException>(() => result.Map<int>(null!));
    }

    [Fact]
    public void Bind_ShouldThrow_WhenBinderIsNull()
    {
        var result = Result.Success<int, BasicError>(42);
        Assert.ThrowsAsync<NullReferenceException>(() => result.Bind<int>(null!));
    }

#endif
}
