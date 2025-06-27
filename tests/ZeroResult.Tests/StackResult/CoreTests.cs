namespace ZeroResult.Tests;

public class CoreTests
{
    [Fact]
    public void SuccessStackResult_ShouldHaveCorrectProperties_WhenCreated()
    {
        var result = StackResult.Success<int, BasicError>(42);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void FailureStackResult_ShouldHaveCorrectProperties_WhenCreated()
    {
        var result = StackResult.Failure<int, BasicError>(TestData.TestError);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(TestData.TestError, result.Error);
    }

    [Fact]
    public void ImplicitConversion_ShouldCreateSuccessStackResult_WhenFromValue()
    {
        StackResult<int, BasicError> result = 42;

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void ImplicitConversion_ShouldCreateFailureStackResult_WhenFromError()
    {
        StackResult<int, BasicError> result = TestData.TestError;

        Assert.True(result.IsFailure);
        Assert.Equal(TestData.TestError, result.Error);
    }

    [Fact]
    public void AccessValue_ShouldThrow_WhenStackResultIsFailure()
    {
        static void Act()
        {
            var result = StackResult.Failure<int, BasicError>(TestData.TestError);
            _ = result.Value;
        }

        var ex = Assert.Throws<InvalidOperationException>(Act);
        Assert.Equal("Cannot access Value on a failed result.", ex.Message);
    }

    [Fact]
    public void AccessError_ShouldThrow_WhenStackResultIsSuccess()
    {
        static void Act()
        {
            var result = StackResult.Success<int, BasicError>(42);
            _ = result.Error;
        }

        var ex = Assert.Throws<InvalidOperationException>(Act);
        Assert.Equal("Cannot access Error on a successful result.", ex.Message);
    }
}
