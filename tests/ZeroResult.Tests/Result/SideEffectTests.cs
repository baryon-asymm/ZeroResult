namespace ZeroResult.Tests;

public class SideEffectTests
{
    [Fact]
    public void OnSuccess_ShouldExecuteAction_WhenSuccess()
    {
        bool executed = false;
        var result = Result.Success<int, BasicError>(42)
            .OnSuccess(x => executed = true);

        Assert.True(executed);
    }

    [Fact]
    public void OnSuccess_ShouldNotExecuteAction_WhenFailure()
    {
        bool executed = false;
        var result = Result.Failure<int, BasicError>(TestData.TestError)
            .OnSuccess(x => executed = true);

        Assert.False(executed);
    }

    [Fact]
    public async Task OnSuccessAsync_ShouldExecuteAction_WhenSuccess()
    {
        bool executed = false;
        var result = await Result.Success<int, BasicError>(42)
            .OnSuccessAsync(_ => { executed = true; return ValueTask.CompletedTask; });

        Assert.True(executed);
    }

    [Fact]
    public void OnFailure_ShouldExecuteAction_WhenFailure()
    {
        bool executed = false;
        var result = Result.Failure<int, BasicError>(TestData.TestError)
            .OnFailure(x => executed = true);

        Assert.True(executed);
    }

    [Fact]
    public void OnFailure_ShouldNotExecuteAction_WhenSuccess()
    {
        bool executed = false;
        var result = Result.Success<int, BasicError>(42)
            .OnFailure(x => executed = true);

        Assert.False(executed);
    }

    [Fact]
    public async Task OnFailureAsync_ShouldExecuteAction_WhenFailure()
    {
        bool executed = false;
        var result = await Result.Failure<int, BasicError>(TestData.TestError)
            .OnFailureAsync(_ => { executed = true; return ValueTask.CompletedTask; });

        Assert.True(executed);
    }
}
