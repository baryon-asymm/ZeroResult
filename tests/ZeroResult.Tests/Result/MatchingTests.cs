namespace ZeroResult.Tests;

public class MatchingTests
{
    [Fact]
    public void Match_ShouldExecuteSuccessBranch_WhenSuccess()
    {
        var result = Result.Success<int, BasicError>(42);
        var output = result.Match(
            onSuccess: x => x.ToString(),
            onFailure: _ => "error");

        Assert.Equal("42", output);
    }

    [Fact]
    public void Match_ShouldExecuteFailureBranch_WhenFailure()
    {
        var result = Result.Failure<int, BasicError>(TestData.TestError);
        var output = result.Match(
            onSuccess: _ => "success",
            onFailure: e => e.Message);

        Assert.Equal("TestError", output);
    }

    [Fact]
    public void MatchAction_ShouldExecuteSuccessAction_WhenSuccess()
    {
        bool successExecuted = false;
        bool failureExecuted = false;

        var result = Result.Success<int, BasicError>(42);
        result.Match(
            onSuccess: _ => successExecuted = true,
            onFailure: _ => failureExecuted = true);

        Assert.True(successExecuted);
        Assert.False(failureExecuted);
    }

    [Fact]
    public void MatchAction_ShouldExecuteFailureAction_WhenFailure()
    {
        bool successExecuted = false;
        bool failureExecuted = false;

        var result = Result.Failure<int, BasicError>(TestData.TestError);
        result.Match(
            onSuccess: _ => successExecuted = true,
            onFailure: _ => failureExecuted = true);

        Assert.False(successExecuted);
        Assert.True(failureExecuted);
    }

    [Fact]
    public async Task MatchAsync_ShouldExecuteSuccessBranch_WhenSuccess()
    {
        var result = Result.Success<int, BasicError>(42);
        var output = await result.MatchAsync(
            onSuccess: x => ValueTask.FromResult(x.ToString()),
            onFailure: _ => ValueTask.FromResult("error"));

        Assert.Equal("42", output);
    }

    [Fact]
    public async Task MatchAsyncAction_ShouldExecuteSuccessAction_WhenSuccess()
    {
        bool successExecuted = false;
        bool failureExecuted = false;

        var result = Result.Success<int, BasicError>(42);
        await result.MatchAsync(
            onSuccess: _ => { successExecuted = true; return ValueTask.CompletedTask; },
            onFailure: _ => { failureExecuted = true; return ValueTask.CompletedTask; });

        Assert.True(successExecuted);
        Assert.False(failureExecuted);
    }
}
