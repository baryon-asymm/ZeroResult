namespace ZeroResult.Tests;

public class ConstructionTests
{
    [Fact]
    public void SuccessResult_ShouldHaveCorrectProperties_WhenCreated()
    {
        var result = Result.Success<int, BasicError>(42);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void FailureResult_ShouldHaveCorrectProperties_WhenCreated()
    {
        var result = Result.Failure<int, BasicError>(TestData.TestError);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(TestData.TestError, result.Error);
    }

    [Fact]
    public void ImplicitConversion_ShouldCreateSuccessResult_WhenFromValue()
    {
        Result<int, BasicError> result = 42;

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void ImplicitConversion_ShouldCreateFailureResult_WhenFromError()
    {
        Result<int, BasicError> result = TestData.TestError;

        Assert.True(result.IsFailure);
        Assert.Equal(TestData.TestError, result.Error);
    }

    [Fact]
    public void AccessValue_ShouldThrow_WhenResultIsFailure()
    {
        var result = Result.Failure<int, BasicError>(TestData.TestError);

        var ex = Assert.Throws<InvalidOperationException>(() => _ = result.Value);
        Assert.Equal("Cannot access Value on a failed result.", ex.Message);
    }

    [Fact]
    public void AccessError_ShouldThrow_WhenResultIsSuccess()
    {
        var result = Result.Success<int, BasicError>(42);

        var ex = Assert.Throws<InvalidOperationException>(() => _ = result.Error);
        Assert.Equal("Cannot access Error on a successful result.", ex.Message);
    }
}
