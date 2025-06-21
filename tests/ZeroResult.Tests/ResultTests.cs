namespace ZeroResult.Tests;

public class ResultTests
{
    private readonly BasicError _testError = new("TestError");
    private readonly BasicError _ensureError = new("EnsureError");

    [Fact]
    public void SuccessResult_HasCorrectProperties()
    {
        var result = Result.Success<int, BasicError>(42);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void FailureResult_HasCorrectProperties()
    {
        var result = Result.Failure<int, BasicError>(_testError);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(_testError, result.Error);
    }

    [Fact]
    public void AccessValueOnFailure_ThrowsException()
    {
        static void Act()
        {
            var result = Result.Failure<int, BasicError>(new BasicError("Test"));
            _ = result.Value;
        }

        var ex = Assert.Throws<InvalidOperationException>(Act);
        Assert.Equal("Cannot access Value on a failed result.", ex.Message);
    }

    [Fact]
    public void AccessErrorOnSuccess_ThrowsException()
    {
        static void Act()
        {
            var result = Result.Success<int, BasicError>(42);
            _ = result.Error;
        }

        var ex = Assert.Throws<InvalidOperationException>(Act);
        Assert.Equal("Cannot access Error on a successful result.", ex.Message);
    }

    [Fact]
    public void Map_OnSuccess_TransformsValue()
    {
        var result = Result.Success<int, BasicError>(42);
        var mapped = result.Map(x => x.ToString());

        Assert.True(mapped.IsSuccess);
        Assert.Equal("42", mapped.Value);
    }

    [Fact]
    public void Map_OnFailure_ReturnsOriginalError()
    {
        var result = Result.Failure<int, BasicError>(_testError);
        var mapped = result.Map(x => x.ToString());

        Assert.False(mapped.IsSuccess);
        Assert.Equal(_testError, mapped.Error);
    }

#if RELEASE

    [Fact]
    public void Map_WithNullMapper_Throws()
    {
        static void Act()
        {
            var result = Result.Success<int, BasicError>(42);
            result.Map<int>(null!);
        }
        
        Assert.Throws<NullReferenceException>(Act);
    }

#endif

    [Fact]
    public void Bind_OnSuccess_TransformsToNewResult()
    {
        static Result<string, BasicError> BindFunc(int value)
        {
            return Result.Success<string, BasicError>(value.ToString());
        }

        var result = Result.Success<int, BasicError>(42);
        var bound = result.Bind(BindFunc);
        
        Assert.True(bound.IsSuccess);
        Assert.Equal("42", bound.Value);
    }
    
    [Fact]
    public void Bind_OnFailure_ReturnsOriginalError()
    {
        var result = Result.Failure<int, BasicError>(_testError);
        var bound = result.Bind(x => Result.Success<string, BasicError>(x.ToString()));
        
        Assert.False(bound.IsSuccess);
        Assert.Equal(_testError, bound.Error);
    }
    
#if RELEASE

    [Fact]
    public void Bind_WithNullBinder_Throws()
    {
        static void Act()
        {
            var result = Result.Success<int, BasicError>(42);
            result.Bind<int>(null!);
        }

        Assert.Throws<NullReferenceException>(Act);
    }

#endif

    [Fact]
    public void Ensure_OnSuccessWithValidPredicate_ReturnsOriginal()
    {
        var result = Result.Success<int, BasicError>(42)
            .Ensure(x => x > 0, () => _ensureError);

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void Ensure_OnSuccessWithInvalidPredicate_ReturnsError()
    {
        var result = Result.Success<int, BasicError>(42)
            .Ensure(x => x < 0, () => _ensureError);

        Assert.False(result.IsSuccess);
        Assert.Equal(_ensureError, result.Error);
    }

    [Fact]
    public void Ensure_OnFailure_ReturnsOriginalError()
    {
        var result = Result.Failure<int, BasicError>(_testError)
            .Ensure(x => x > 0, () => _ensureError);

        Assert.False(result.IsSuccess);
        Assert.Equal(_testError, result.Error);
    }

    [Fact]
    public void OnSuccess_ExecutesAction_WhenSuccess()
    {
        bool executed = false;
        var result = Result.Success<int, BasicError>(42)
            .OnSuccess(x => executed = true);

        Assert.True(executed);
    }

    [Fact]
    public void OnSuccess_DoesNotExecuteAction_WhenFailure()
    {
        bool executed = false;
        var result = Result.Failure<int, BasicError>(_testError)
            .OnSuccess(x => executed = true);

        Assert.False(executed);
    }

    [Fact]
    public void OnFailure_ExecutesAction_WhenFailure()
    {
        bool executed = false;
        var result = Result.Failure<int, BasicError>(_testError)
            .OnFailure(x => executed = true);

        Assert.True(executed);
    }

    [Fact]
    public void OnFailure_DoesNotExecuteAction_WhenSuccess()
    {
        bool executed = false;
        var result = Result.Success<int, BasicError>(42)
            .OnFailure(x => executed = true);

        Assert.False(executed);
    }

    [Fact]
    public void Match_OnSuccess_ExecutesSuccessBranch()
    {
        var result = Result.Success<int, BasicError>(42);
        var output = result.Match(
            onSuccess: x => x.ToString(),
            onFailure: _ => "error");

        Assert.Equal("42", output);
    }

    [Fact]
    public void Match_OnFailure_ExecutesFailureBranch()
    {
        var result = Result.Failure<int, BasicError>(_testError);
        var output = result.Match(
            onSuccess: _ => "success",
            onFailure: e => e.Message);

        Assert.Equal("TestError", output);
    }

    [Fact]
    public void MatchAction_OnSuccess_ExecutesSuccessAction()
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
    public void MatchAction_OnFailure_ExecutesFailureAction()
    {
        bool successExecuted = false;
        bool failureExecuted = false;

        var result = Result.Failure<int, BasicError>(_testError);
        result.Match(
            onSuccess: _ => successExecuted = true,
            onFailure: _ => failureExecuted = true);

        Assert.False(successExecuted);
        Assert.True(failureExecuted);
    }

    [Fact]
    public void Equality_ComparesCorrectly()
    {
        var success1 = Result.Success<int, BasicError>(42);
        var success2 = Result.Success<int, BasicError>(42);
        var successDifferent = Result.Success<int, BasicError>(100);
        var failure1 = Result.Failure<int, BasicError>(_testError);
        var failure2 = Result.Failure<int, BasicError>(_testError);
        var failureDifferent = Result.Failure<int, BasicError>(_ensureError);

        // Equal successes
        Assert.True(success1 == success2);
        Assert.False(success1 != success2);
        Assert.True(success1.Equals(success2));

        // Different successes
        Assert.False(success1 == successDifferent);
        Assert.True(success1 != successDifferent);
        Assert.False(success1.Equals(successDifferent));

        // Equal failures
        Assert.True(failure1 == failure2);
        Assert.False(failure1 != failure2);
        Assert.True(failure1.Equals(failure2));

        // Different failures
        Assert.False(failure1 == failureDifferent);
        Assert.True(failure1 != failureDifferent);
        Assert.False(failure1.Equals(failureDifferent));

        // Success vs failure
        Assert.False(success1 == failure1);
        Assert.True(success1 != failure1);
        Assert.False(success1.Equals(failure1));
    }
}
