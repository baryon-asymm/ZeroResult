using System.Runtime.CompilerServices;
using ZeroResult.Core.Errors;
using ZeroResult.Core.Models;

namespace ZeroResult.Tests.Core.Models;

public class StackResultTests
{
    private readonly BasicError _testError = new("TestError");
    private readonly BasicError _ensureError = new("EnsureError");

    [Fact]
    public void SuccessResult_HasCorrectProperties()
    {
        var result = StackResult.Success<int, BasicError>(42);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void FailureResult_HasCorrectProperties()
    {
        var result = StackResult.Failure<int, BasicError>(_testError);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(_testError, result.Error);
    }

    [Fact]
    public void AccessValueOnFailure_ThrowsException()
    {
        static void Act()
        {
            var result = StackResult.Failure<int, BasicError>(new BasicError("Test"));
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
            var result = StackResult.Success<int, BasicError>(42);
            _ = result.Error;
        }

        var ex = Assert.Throws<InvalidOperationException>(Act);
        Assert.Equal("Cannot access Error on a successful result.", ex.Message);
    }

    [Fact]
    public void Map_OnSuccess_TransformsValue()
    {
        var result = StackResult.Success<int, BasicError>(42);
        var mapped = result.Map(x => x.ToString());

        Assert.True(mapped.IsSuccess);
        Assert.Equal("42", mapped.Value);
    }

    [Fact]
    public void Map_OnFailure_ReturnsOriginalError()
    {
        var result = StackResult.Failure<int, BasicError>(_testError);
        var mapped = result.Map(x => x.ToString());

        Assert.False(mapped.IsSuccess);
        Assert.Equal(_testError, mapped.Error);
    }

#if RELEASE
    
    // In release mode, we expect a NullReferenceException if the mapper is null.
    // Skip this test in debug mode to avoid DebugAssertException.
    [Fact]
    public void Map_WithNullMapper_Throws()
    {
        static void Act()
        {
            var result = StackResult.Success<int, BasicError>(42);
            result.Map<int>(null!);
        }
        
        Assert.Throws<NullReferenceException>(Act);
    }

#endif

#if NET9_0_OR_GREATER

    [Fact]
    public void Bind_OnSuccess_TransformsToNewResult()
    {
        static StackResult<string, BasicError> BindFunc(int value)
        {
            return StackResult.Success<string, BasicError>(value.ToString());
        }

        var result = StackResult.Success<int, BasicError>(42);
        var bound = result.Bind(BindFunc);
        
        Assert.True(bound.IsSuccess);
        Assert.Equal("42", bound.Value);
    }
    
    [Fact]
    public void Bind_OnFailure_ReturnsOriginalError()
    {
        var result = StackResult.Failure<int, BasicError>(_testError);
        var bound = result.Bind(x => StackResult.Success<string, BasicError>(x.ToString()));
        
        Assert.False(bound.IsSuccess);
        Assert.Equal(_testError, bound.Error);
    }
    
#if RELEASE

    // In release mode, we expect a NullReferenceException if the binder is null.
    // Skip this test in debug mode to avoid DebugAssertException.
    [Fact]
    public void Bind_WithNullBinder_Throws()
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

    [Fact]
    public void Ensure_OnSuccessWithValidPredicate_ReturnsOriginal()
    {
        var result = StackResult.Success<int, BasicError>(42)
            .Ensure(x => x > 0, () => _ensureError);

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void Ensure_OnSuccessWithInvalidPredicate_ReturnsError()
    {
        var result = StackResult.Success<int, BasicError>(42)
            .Ensure(x => x < 0, () => _ensureError);

        Assert.False(result.IsSuccess);
        Assert.Equal(_ensureError, result.Error);
    }

    [Fact]
    public void Ensure_OnFailure_ReturnsOriginalError()
    {
        var result = StackResult.Failure<int, BasicError>(_testError)
            .Ensure(x => x > 0, () => _ensureError);

        Assert.False(result.IsSuccess);
        Assert.Equal(_testError, result.Error);
    }

    [Fact]
    public void OnSuccess_ExecutesAction_WhenSuccess()
    {
        bool executed = false;
        var result = StackResult.Success<int, BasicError>(42)
            .OnSuccess(x => executed = true);

        Assert.True(executed);
    }

    [Fact]
    public void OnSuccess_DoesNotExecuteAction_WhenFailure()
    {
        bool executed = false;
        var result = StackResult.Failure<int, BasicError>(_testError)
            .OnSuccess(x => executed = true);

        Assert.False(executed);
    }

    [Fact]
    public void OnFailure_ExecutesAction_WhenFailure()
    {
        bool executed = false;
        var result = StackResult.Failure<int, BasicError>(_testError)
            .OnFailure(x => executed = true);

        Assert.True(executed);
    }

    [Fact]
    public void OnFailure_DoesNotExecuteAction_WhenSuccess()
    {
        bool executed = false;
        var result = StackResult.Success<int, BasicError>(42)
            .OnFailure(x => executed = true);

        Assert.False(executed);
    }

    [Fact]
    public void Match_OnSuccess_ExecutesSuccessBranch()
    {
        var result = StackResult.Success<int, BasicError>(42);
        var output = result.Match(
            onSuccess: x => x.ToString(),
            onFailure: _ => "error");

        Assert.Equal("42", output);
    }

    [Fact]
    public void Match_OnFailure_ExecutesFailureBranch()
    {
        var result = StackResult.Failure<int, BasicError>(_testError);
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

        var result = StackResult.Success<int, BasicError>(42);
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

        var result = StackResult.Failure<int, BasicError>(_testError);
        result.Match(
            onSuccess: _ => successExecuted = true,
            onFailure: _ => failureExecuted = true);

        Assert.False(successExecuted);
        Assert.True(failureExecuted);
    }
}

public class BasicError(string message) : IError
{
    public string Message { get; } = message;

    public string? Code => throw new NotImplementedException();
}
