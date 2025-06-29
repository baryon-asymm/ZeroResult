namespace ZeroResult.Tests;

public class OperationsTests
{
    [Fact]
    public void Map_ShouldTransformValue_WhenSuccess()
    {
        var result = StackResult.Success<int, BasicError>(42);
        var mapped = result.Map(x => x.ToString());

        Assert.True(mapped.IsSuccess);
        Assert.Equal("42", mapped.Value);
    }

    [Fact]
    public void Map_ShouldReturnOriginalError_WhenFailure()
    {
        var result = StackResult.Failure<int, BasicError>(TestData.TestError);
        var mapped = result.Map(x => x.ToString());

        Assert.False(mapped.IsSuccess);
        Assert.Equal(TestData.TestError, mapped.Error);
    }

#if NET9_0_OR_GREATER
    [Fact]
    public void Bind_ShouldTransformToNewStackResult_WhenSuccess()
    {
        var result = StackResult.Success<int, BasicError>(42);
        var bound = result.Bind(x => StackResult.Success<string, BasicError>(x.ToString()));

        Assert.True(bound.IsSuccess);
        Assert.Equal("42", bound.Value);
    }

    [Fact]
    public void Bind_ShouldReturnOriginalError_WhenFailure()
    {
        var result = StackResult.Failure<int, BasicError>(TestData.TestError);
        var bound = result.Bind(x => StackResult.Success<string, BasicError>(x.ToString()));

        Assert.False(bound.IsSuccess);
        Assert.Equal(TestData.TestError, bound.Error);
    }
#endif

    [Fact]
    public void Ensure_ShouldReturnOriginal_WhenSuccessAndPredicateTrue()
    {
        var result = StackResult.Success<int, BasicError>(42)
            .Ensure(x => x > 0, () => TestData.EnsureError);

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void Ensure_ShouldReturnError_WhenSuccessAndPredicateFalse()
    {
        var result = StackResult.Success<int, BasicError>(42)
            .Ensure(x => x < 0, () => TestData.EnsureError);

        Assert.False(result.IsSuccess);
        Assert.Equal(TestData.EnsureError, result.Error);
    }

    [Fact]
    public void Ensure_ShouldReturnOriginalError_WhenFailure()
    {
        var result = StackResult.Failure<int, BasicError>(TestData.TestError)
            .Ensure(x => x > 0, () => TestData.EnsureError);

        Assert.False(result.IsSuccess);
        Assert.Equal(TestData.TestError, result.Error);
    }

    [Fact]
    public void OnSuccess_ShouldExecuteAction_WhenSuccess()
    {
        bool executed = false;
        var result = StackResult.Success<int, BasicError>(42)
            .OnSuccess(x => executed = true);

        Assert.True(executed);
    }

    [Fact]
    public void OnSuccess_ShouldNotExecuteAction_WhenFailure()
    {
        bool executed = false;
        var result = StackResult.Failure<int, BasicError>(TestData.TestError)
            .OnSuccess(x => executed = true);

        Assert.False(executed);
    }

    [Fact]
    public void OnFailure_ShouldExecuteAction_WhenFailure()
    {
        bool executed = false;
        var result = StackResult.Failure<int, BasicError>(TestData.TestError)
            .OnFailure(x => executed = true);

        Assert.True(executed);
    }

    [Fact]
    public void OnFailure_ShouldNotExecuteAction_WhenSuccess()
    {
        bool executed = false;
        var result = StackResult.Success<int, BasicError>(42)
            .OnFailure(x => executed = true);

        Assert.False(executed);
    }

    [Fact]
    public void Match_ShouldExecuteSuccessBranch_WhenSuccess()
    {
        var result = StackResult.Success<int, BasicError>(42);
        var output = result.Match(
            onSuccess: x => x.ToString(),
            onFailure: _ => "error");

        Assert.Equal("42", output);
    }

    [Fact]
    public void Match_ShouldExecuteFailureBranch_WhenFailure()
    {
        var result = StackResult.Failure<int, BasicError>(TestData.TestError);
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

        var result = StackResult.Success<int, BasicError>(42);
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

        var result = StackResult.Failure<int, BasicError>(TestData.TestError);
        result.Match(
            onSuccess: _ => successExecuted = true,
            onFailure: _ => failureExecuted = true);

        Assert.False(successExecuted);
        Assert.True(failureExecuted);
    }

#if RELEASE

    [Fact]
    public void Map_ShouldThrow_WhenMapperIsNull()
    {
        static void Act()
        {
            var result = StackResult.Success<int, BasicError>(42);
            result.Map<int>(null!);
        }
        
        Assert.Throws<NullReferenceException>(Act);
    }

#if NET9_0_OR_GREATER

    [Fact]
    public void Bind_ShouldThrow_WhenBinderIsNull()
    {
        static void Act()
        {
            var result = StackResult.Success<int, BasicError>(42);
            result.Bind<int>(null!);
        }
        
        Assert.Throws<NullReferenceException>(Act);
    }

#endif

#endif
}
