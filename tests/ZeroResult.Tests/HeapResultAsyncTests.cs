namespace ZeroResult.Tests;

public class HeapResultAsyncTests
{
    private readonly BasicError _testError = new("TestError");
    private readonly BasicError _ensureError = new("EnsureError");

    [Fact]
    public async Task MapAsync_OnSuccess_TransformsValue()
    {
        var result = HeapResult.Success<int, BasicError>(42);
        var mapped = await result.MapAsync(x => ValueTask.FromResult(x.ToString()));

        Assert.True(mapped.IsSuccess);
        Assert.Equal("42", mapped.Value);
    }

    [Fact]
    public async Task MapAsync_OnFailure_ReturnsOriginalError()
    {
        var result = HeapResult.Failure<int, BasicError>(_testError);
        var mapped = await result.MapAsync(x => ValueTask.FromResult(x.ToString()));

        Assert.False(mapped.IsSuccess);
        Assert.Equal(_testError, mapped.Error);
    }

#if RELEASE

    [Fact]
    public async Task MapAsync_WithNullMapper_Throws()
    {
        async Task Act()
        {
            var result = HeapResult.Success<int, BasicError>(42);
            await result.MapAsync<int>(null!);
        }
        
        await Assert.ThrowsAsync<NullReferenceException>(Act);
    }

#endif

    [Fact]
    public async Task BindAsync_OnSuccess_TransformsToNewResult()
    {
        static ValueTask<HeapResult<string, BasicError>> BindFunc(int value)
        {
            return ValueTask.FromResult(HeapResult.Success<string, BasicError>(value.ToString()));
        }

        var result = HeapResult.Success<int, BasicError>(42);
        var bound = await result.BindAsync(BindFunc);

        Assert.True(bound.IsSuccess);
        Assert.Equal("42", bound.Value);
    }

    [Fact]
    public async Task BindAsync_OnFailure_ReturnsOriginalError()
    {
        var result = HeapResult.Failure<int, BasicError>(_testError);
        var bound = await result.BindAsync(x =>
            ValueTask.FromResult(HeapResult.Success<string, BasicError>(x.ToString())));

        Assert.False(bound.IsSuccess);
        Assert.Equal(_testError, bound.Error);
    }

#if RELEASE

    [Fact]
    public async Task BindAsync_WithNullBinder_Throws()
    {
        async Task Act()
        {
            var result = HeapResult.Success<int, BasicError>(42);
            await result.BindAsync<int>(null!);
        }

        await Assert.ThrowsAsync<NullReferenceException>(Act);
    }

#endif

    [Fact]
    public async Task EnsureAsync_OnSuccessWithValidPredicate_ReturnsOriginal()
    {
        var result = await HeapResult.Success<int, BasicError>(42)
            .EnsureAsync(
                x => ValueTask.FromResult(x > 0),
                () => ValueTask.FromResult(_ensureError));

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public async Task EnsureAsync_OnSuccessWithInvalidPredicate_ReturnsError()
    {
        var result = await HeapResult.Success<int, BasicError>(42)
            .EnsureAsync(
                x => ValueTask.FromResult(x < 0),
                () => ValueTask.FromResult(_ensureError));

        Assert.False(result.IsSuccess);
        Assert.Equal(_ensureError, result.Error);
    }

    [Fact]
    public async Task EnsureAsync_OnFailure_ReturnsOriginalError()
    {
        var result = await HeapResult.Failure<int, BasicError>(_testError)
            .EnsureAsync(
                x => ValueTask.FromResult(x > 0),
                () => ValueTask.FromResult(_ensureError));

        Assert.False(result.IsSuccess);
        Assert.Equal(_testError, result.Error);
    }

    [Fact]
    public async Task OnSuccessAsync_ExecutesAction_WhenSuccess()
    {
        bool executed = false;
        var result = await HeapResult.Success<int, BasicError>(42)
            .OnSuccessAsync(x =>
            {
                executed = true;
                return ValueTask.CompletedTask;
            });

        Assert.True(executed);
    }

    [Fact]
    public async Task OnSuccessAsync_DoesNotExecuteAction_WhenFailure()
    {
        bool executed = false;
        var result = await HeapResult.Failure<int, BasicError>(_testError)
            .OnSuccessAsync(x =>
            {
                executed = true;
                return ValueTask.CompletedTask;
            });

        Assert.False(executed);
    }

    [Fact]
    public async Task OnFailureAsync_ExecutesAction_WhenFailure()
    {
        bool executed = false;
        var result = await HeapResult.Failure<int, BasicError>(_testError)
            .OnFailureAsync(x =>
            {
                executed = true;
                return ValueTask.CompletedTask;
            });

        Assert.True(executed);
    }

    [Fact]
    public async Task OnFailureAsync_DoesNotExecuteAction_WhenSuccess()
    {
        bool executed = false;
        var result = await HeapResult.Success<int, BasicError>(42)
            .OnFailureAsync(x =>
            {
                executed = true;
                return ValueTask.CompletedTask;
            });

        Assert.False(executed);
    }

    [Fact]
    public async Task MatchAsync_OnSuccess_ExecutesSuccessBranch()
    {
        var result = HeapResult.Success<int, BasicError>(42);
        var output = await result.MatchAsync(
            onSuccess: x => ValueTask.FromResult(x.ToString()),
            onFailure: _ => ValueTask.FromResult("error"));

        Assert.Equal("42", output);
    }

    [Fact]
    public async Task MatchAsync_OnFailure_ExecutesFailureBranch()
    {
        var result = HeapResult.Failure<int, BasicError>(_testError);
        var output = await result.MatchAsync(
            onSuccess: _ => ValueTask.FromResult("success"),
            onFailure: e => ValueTask.FromResult(e.Message));

        Assert.Equal("TestError", output);
    }

    [Fact]
    public async Task MatchActionAsync_OnSuccess_ExecutesSuccessAction()
    {
        bool successExecuted = false;
        bool failureExecuted = false;

        var result = HeapResult.Success<int, BasicError>(42);
        await result.MatchAsync(
            onSuccess: _ =>
            {
                successExecuted = true;
                return ValueTask.CompletedTask;
            },
            onFailure: _ =>
            {
                failureExecuted = true;
                return ValueTask.CompletedTask;
            });

        Assert.True(successExecuted);
        Assert.False(failureExecuted);
    }

    [Fact]
    public async Task MatchActionAsync_OnFailure_ExecutesFailureAction()
    {
        bool successExecuted = false;
        bool failureExecuted = false;

        var result = HeapResult.Failure<int, BasicError>(_testError);
        await result.MatchAsync(
            onSuccess: _ =>
            {
                successExecuted = true;
                return ValueTask.CompletedTask;
            },
            onFailure: _ =>
            {
                failureExecuted = true;
                return ValueTask.CompletedTask;
            });

        Assert.False(successExecuted);
        Assert.True(failureExecuted);
    }

    // Additional edge case tests
    [Fact]
    public async Task AsyncOperations_WithDelayedTasks_CompleteCorrectly()
    {
        async ValueTask<int> DelayedSuccess(int value)
        {
            await Task.Delay(10);
            return value;
        }

        var result = HeapResult.Success<int, BasicError>(42);
        var mapped = await result.MapAsync(DelayedSuccess);

        Assert.True(mapped.IsSuccess);
        Assert.Equal(42, mapped.Value);
    }

    [Fact]
    public async Task AsyncOperations_WithFaultedTask_PropagatesException()
    {
        static ValueTask<int> FaultedMap(int value)
        {
            return ValueTask.FromException<int>(new InvalidOperationException("Test exception"));
        }

        var result = HeapResult.Success<int, BasicError>(42);
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            result.MapAsync(FaultedMap).AsTask());
    }
}
