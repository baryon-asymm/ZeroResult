namespace ZeroResult.Tests;

public class ConversionTests
{
    [Fact]
    public void AsResult_ShouldConvertToResult_WhenSuccess()
    {
        var stackResult = StackResult.Success<int, BasicError>(42);
        var result = stackResult.AsResult();

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void AsResult_ShouldConvertToResult_WhenFailure()
    {
        var stackResult = StackResult.Failure<int, BasicError>(TestData.TestError);
        var result = stackResult.AsResult();

        Assert.True(result.IsFailure);
        Assert.Equal(TestData.TestError, result.Error);
    }

    [Fact]
    public void RoundtripConversion_ShouldPreserveState_WhenSuccess()
    {
        var original = StackResult.Success<int, BasicError>(42);
        var result = original.AsResult();
        var backToStack = result.AsStackResult();

        Assert.True(backToStack.IsSuccess);
        Assert.Equal(original.Value, backToStack.Value);
    }

    [Fact]
    public void RoundtripConversion_ShouldPreserveState_WhenFailure()
    {
        var original = StackResult.Failure<int, BasicError>(TestData.TestError);
        var result = original.AsResult();
        var backToStack = result.AsStackResult();

        Assert.True(backToStack.IsFailure);
        Assert.Equal(original.Error, backToStack.Error);
    }
}
