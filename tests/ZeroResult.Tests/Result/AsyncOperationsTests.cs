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
    public async Task ImplicitConversion_ShouldWork_ForTask()
    {
        Task<Result<int, BasicError>> task = Result.Success<int, BasicError>(42);
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

    [Fact]
    public async Task MapAsync_ShouldTransformValue_ForSuccess()
    {
        var result = await Result.Success<int, BasicError>(42)
            .MapAsync(x => ValueTask.FromResult(x * 2));

        Assert.True(result.IsSuccess);
        Assert.Equal(84, result.Value);
    }

    [Fact]
    public async Task MapAsync_ShouldPreserveError_ForFailure()
    {
        var result = await Result.Failure<int, BasicError>(TestData.TestError)
            .MapAsync(x => ValueTask.FromResult(x * 2));

        Assert.False(result.IsSuccess);
        Assert.Equal(TestData.TestError, result.Error);
    }

    [Fact]
    public async Task BindAsync_ShouldChainOperations_ForSuccess()
    {
        var result = await Result.Success<int, BasicError>(42)
            .BindAsync(x => ValueTask.FromResult(Result.Success<int, BasicError>(x * 2)));

        Assert.True(result.IsSuccess);
        Assert.Equal(84, result.Value);
    }

    [Fact]
    public async Task BindAsync_ShouldShortCircuit_ForFailure()
    {
        var result = await Result.Failure<int, BasicError>(TestData.TestError)
            .BindAsync(x => ValueTask.FromResult(Result.Success<int, BasicError>(x * 2)));

        Assert.False(result.IsSuccess);
        Assert.Equal(TestData.TestError, result.Error);
    }

    [Fact]
    public async Task EnsureAsync_ShouldPass_WhenPredicateIsTrue()
    {
        var result = await Result.Success<int, BasicError>(42)
            .EnsureAsync(x => ValueTask.FromResult(x > 0), () => ValueTask.FromResult(TestData.TestError));

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public async Task EnsureAsync_ShouldFail_WhenPredicateIsFalse()
    {
        var result = await Result.Success<int, BasicError>(42)
            .EnsureAsync(x => ValueTask.FromResult(x < 0), () => ValueTask.FromResult(TestData.TestError));

        Assert.False(result.IsSuccess);
        Assert.Equal(TestData.TestError, result.Error);
    }

    [Fact]
    public async Task EnsureAsync_ShouldPreserveError_ForFailure()
    {
        var result = await Result.Failure<int, BasicError>(TestData.TestError)
            .EnsureAsync(x => ValueTask.FromResult(x > 0), () => ValueTask.FromResult(new BasicError("NewError")));

        Assert.False(result.IsSuccess);
        Assert.Equal(TestData.TestError, result.Error);
    }

    [Fact]
    public async Task OnSuccessAsync_ShouldExecuteAction_ForSuccess()
    {
        bool wasCalled = false;
        var result = await Result.Success<int, BasicError>(42)
            .OnSuccessAsync(x =>
            {
                wasCalled = true;
                return ValueTask.CompletedTask;
            });

        Assert.True(wasCalled);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task OnSuccessAsync_ShouldNotExecuteAction_ForFailure()
    {
        bool wasCalled = false;
        var result = await Result.Failure<int, BasicError>(TestData.TestError)
            .OnSuccessAsync(x =>
            {
                wasCalled = true;
                return ValueTask.CompletedTask;
            });

        Assert.False(wasCalled);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task OnFailureAsync_ShouldExecuteAction_ForFailure()
    {
        bool wasCalled = false;
        var result = await Result.Failure<int, BasicError>(TestData.TestError)
            .OnFailureAsync(error =>
            {
                wasCalled = true;
                return ValueTask.CompletedTask;
            });

        Assert.True(wasCalled);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task OnFailureAsync_ShouldNotExecuteAction_ForSuccess()
    {
        bool wasCalled = false;
        var result = await Result.Success<int, BasicError>(42)
            .OnFailureAsync(error =>
            {
                wasCalled = true;
                return ValueTask.CompletedTask;
            });

        Assert.False(wasCalled);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task MatchAsync_ShouldExecuteSuccessBranch_ForSuccess()
    {
        var result = await Result.Success<int, BasicError>(42)
            .MatchAsync(
                x => ValueTask.FromResult(x * 2),
                error => ValueTask.FromResult(0));

        Assert.Equal(84, result);
    }

    [Fact]
    public async Task MatchAsync_ShouldExecuteFailureBranch_ForFailure()
    {
        var result = await Result.Failure<int, BasicError>(TestData.TestError)
            .MatchAsync(
                x => ValueTask.FromResult(x * 2),
                error => ValueTask.FromResult(0));

        Assert.Equal(0, result);
    }

    [Fact]
    public async Task MatchAsync_WithAction_ShouldExecuteSuccessBranch_ForSuccess()
    {
        bool successCalled = false;
        bool failureCalled = false;

        await Result.Success<int, BasicError>(42)
            .MatchAsync(
                x =>
                {
                    successCalled = true;
                    return ValueTask.CompletedTask;
                },
                error =>
                {
                    failureCalled = true;
                    return ValueTask.CompletedTask;
                });

        Assert.True(successCalled);
        Assert.False(failureCalled);
    }

    [Fact]
    public async Task MatchAsync_WithAction_ShouldExecuteFailureBranch_ForFailure()
    {
        bool successCalled = false;
        bool failureCalled = false;

        await Result.Failure<int, BasicError>(TestData.TestError)
            .MatchAsync(
                x =>
                {
                    successCalled = true;
                    return ValueTask.CompletedTask;
                },
                error =>
                {
                    failureCalled = true;
                    return ValueTask.CompletedTask;
                });

        Assert.False(successCalled);
        Assert.True(failureCalled);
    }

    [Fact]
    public async Task TaskExtensions_MapAsync_ShouldWork()
    {
        var result = await Task.FromResult(Result.Success<int, BasicError>(42))
            .MapAsync(x => Task.FromResult(x.ToString()));

        Assert.True(result.IsSuccess);
        Assert.Equal("42", result.Value);
    }

    [Fact]
    public async Task TaskExtensions_BindAsync_ShouldWork()
    {
        var result = await Task.FromResult(Result.Success<int, BasicError>(42))
            .BindAsync(x => Task.FromResult(Result.Success<string, BasicError>(x.ToString())));

        Assert.True(result.IsSuccess);
        Assert.Equal("42", result.Value);
    }

    [Fact]
    public async Task TaskExtensions_EnsureAsync_ShouldWork()
    {
        var result = await Task.FromResult(Result.Success<int, BasicError>(42))
            .EnsureAsync(x => Task.FromResult(x > 0), () => Task.FromResult(TestData.TestError));

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public async Task TaskExtensions_TapAsync_ShouldExecuteAction_WithoutModifyingResult()
    {
        bool wasCalled = false;
        var result = await Task.FromResult(Result.Success<int, BasicError>(42))
            .TapAsync(x =>
            {
                wasCalled = true;
                return Task.CompletedTask;
            });

        Assert.True(wasCalled);
        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public async Task ValueTaskExtensions_TapAsync_ShouldExecuteAction_WithoutModifyingResult()
    {
        bool wasCalled = false;
        var result = await ValueTask.FromResult(Result.Success<int, BasicError>(42))
            .TapAsync(x =>
            {
                wasCalled = true;
                return ValueTask.CompletedTask;
            });

        Assert.True(wasCalled);
        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }
}
