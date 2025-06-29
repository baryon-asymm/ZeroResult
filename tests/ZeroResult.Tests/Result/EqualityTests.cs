namespace ZeroResult.Tests;

public class EqualityTests
{
    [Theory]
    [InlineData(42, 42, true)]
    [InlineData(42, 100, false)]
    public void EqualityOperator_ShouldCompareCorrectly_ForSuccess(int value1, int value2, bool expected)
    {
        var result1 = Result.Success<int, BasicError>(value1);
        var result2 = Result.Success<int, BasicError>(value2);

        Assert.Equal(expected, result1 == result2);
        Assert.Equal(!expected, result1 != result2);
    }

    [Fact]
    public void EqualityOperator_ShouldReturnFalse_WhenComparingSuccessWithFailure()
    {
        var success = Result.Success<int, BasicError>(42);
        var failure = Result.Failure<int, BasicError>(TestData.TestError);

        Assert.False(success == failure);
        Assert.True(success != failure);
    }

    [Theory]
    [InlineData("TestError", "TestError", true)]
    [InlineData("TestError", "OtherError", false)]
    public void EqualityOperator_ShouldCompareCorrectly_ForFailure(string error1, string error2, bool expected)
    {
        var result1 = Result.Failure<int, BasicError>(new BasicError(error1));
        var result2 = Result.Failure<int, BasicError>(new BasicError(error2));

        Assert.Equal(expected, result1 == result2);
        Assert.Equal(!expected, result1 != result2);
    }

    [Fact]
    public void EqualsMethod_ShouldReturnTrue_ForSameReference()
    {
        var result = Result.Success<int, BasicError>(42);
        Assert.True(result.Equals(result));
    }

    [Fact]
    public void EqualsMethod_ShouldReturnFalse_ForNull()
    {
        var result = Result.Success<int, BasicError>(42);
        Assert.False(result.Equals(null!));
    }
}
