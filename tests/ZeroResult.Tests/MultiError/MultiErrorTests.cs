using System.Collections.ObjectModel;
using ZeroResult.Errors;

namespace ZeroResult.Tests;

public class MultiErrorTests
{
    private static readonly TestError TestError1 = new("Error1", "CODE1");
    private static readonly TestError TestError2 = new("Error2", "CODE2");
    private static readonly TestError TestError3 = new("Error3");

    [Fact]
    public void Constructor_WithErrorArray_InitializesCorrectly()
    {
        var errors = new IError[] { TestError1, TestError2 };
        var multiError = new MultiError(errors);

        Assert.Equal(2, multiError.Count);
        Assert.Equal(TestError1, multiError.GetErrorAt(0));
        Assert.Equal(TestError2, multiError.GetErrorAt(1));
    }

    [Fact]
    public void Constructor_WithReadOnlyCollection_InitializesCorrectly()
    {
        var errors = new List<IError> { TestError1, TestError2 }.AsReadOnly();
        var multiError = new MultiError(errors);

        Assert.Equal(2, multiError.Count);
        Assert.Equal(TestError1, multiError.GetErrorAt(0));
        Assert.Equal(TestError2, multiError.GetErrorAt(1));
    }

    [Fact]
    public void Constructor_WithEmptyArray_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new MultiError(Array.Empty<IError>()));
    }

    [Fact]
    public void Constructor_WithNullArray_ThrowsArgumentNullException()
    {
        var nullArray = (IError[]?)null;
        Assert.Throws<ArgumentNullException>(() => new MultiError(nullArray!));
    }

    [Fact]
    public void Constructor_WithEmptyReadOnlyCollection_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new MultiError(new List<IError>().AsReadOnly()));
    }

    [Fact]
    public void Constructor_WithNullReadOnlyCollection_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new MultiError((ReadOnlyCollection<IError>)null!));
    }

    [Fact]
    public void Message_WithSingleError_ReturnsSingleErrorMessage()
    {
        var multiError = new MultiError([TestError1]);
        Assert.Equal(TestError1.Message, multiError.Message);
    }

    [Fact]
    public void Message_WithMultipleErrors_ReturnsCombinedMessage()
    {
        var multiError = new MultiError([TestError1, TestError2]);
        var message = multiError.Message;

        Assert.Contains("Multiple errors occurred (2)", message);
        Assert.Contains($"- {TestError1.Message} (Code: {TestError1.Code})", message);
        Assert.Contains($"- {TestError2.Message} (Code: {TestError2.Code})", message);
    }

    [Fact]
    public void Message_WithErrorWithoutCode_OmitsCodeInMessage()
    {
        var multiError = new MultiError([TestError3]);
        Assert.Equal(TestError3.Message, multiError.Message);
    }

    [Fact]
    public void Code_WithSingleError_ReturnsFirstErrorCode()
    {
        var multiError = new MultiError([TestError1]);
        Assert.Equal(TestError1.Code, multiError.Code);
    }

    [Fact]
    public void Code_WithMultipleErrors_ReturnsFirstErrorCode()
    {
        var multiError = new MultiError([TestError1, TestError2]);
        Assert.Equal(TestError1.Code, multiError.Code);
    }

    [Fact]
    public void Code_WithErrorWithoutCode_ReturnsNull()
    {
        var multiError = new MultiError([TestError3]);
        Assert.Null(multiError.Code);
    }

    [Fact]
    public void AsEnumerable_ReturnsAllErrors()
    {
        var errors = new[] { TestError1, TestError2 };
        var multiError = new MultiError(errors);
        
        Assert.Equal(errors, multiError.AsEnumerable());
    }

    [Fact]
    public void GetErrorAt_WithInvalidIndex_ThrowsArgumentOutOfRangeException()
    {
        var multiError = new MultiError([TestError1]);
        
        Assert.Throws<ArgumentOutOfRangeException>(() => multiError.GetErrorAt(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => multiError.GetErrorAt(1));
    }

    [Fact]
    public void CreateBuilder_CreatesValidBuilder()
    {
        var builder = MultiError.CreateBuilder();
        builder.Add(TestError1);
        builder.Add(TestError2);
        
        var multiError = builder.Build();
        
        Assert.Equal(2, multiError.Count);
        Assert.Equal(TestError1, multiError.GetErrorAt(0));
        Assert.Equal(TestError2, multiError.GetErrorAt(1));
    }

    [Fact]
    public void CreateBuilder_WithInitialCapacity_RespectsCapacity()
    {
        var builder = MultiError.CreateBuilder(10);
        builder.Add(TestError1);
        
        var multiError = builder.Build();
        
        Assert.Equal(1, multiError.Count);
    }

    [Fact]
    public void Builder_AddRange_AddsAllErrors()
    {
        IError[] errors = [TestError1, TestError2];
        var builder = MultiError.CreateBuilder();
        builder.AddRange(errors.AsSpan());
        
        var multiError = builder.Build();
        
        Assert.Equal(2, multiError.Count);
        Assert.Equal(errors, multiError.AsEnumerable());
    }

    [Fact]
    public void Merge_CombinesErrorsFromBothInstances()
    {
        var first = new MultiError([TestError1]);
        var second = new MultiError([TestError2]);
        
        var merged = MultiError.Merge(first, second);
        
        Assert.Equal(2, merged.Count);
        Assert.Equal(TestError1, merged.GetErrorAt(0));
        Assert.Equal(TestError2, merged.GetErrorAt(1));
    }

    [Fact]
    public void Merge_WithNullFirstArgument_ThrowsArgumentNullException()
    {
        var valid = new MultiError([TestError1]);
        
        Assert.Throws<ArgumentNullException>(() => MultiError.Merge(null!, valid));
    }

    [Fact]
    public void Merge_WithNullSecondArgument_ThrowsArgumentNullException()
    {
        var valid = new MultiError([TestError1]);
        
        Assert.Throws<ArgumentNullException>(() => MultiError.Merge(valid, null!));
    }

    private sealed class TestError : IError
    {
        public TestError(string message, string? code = null)
        {
            Message = message;
            Code = code;
        }

        public string Message { get; }
        public string? Code { get; }
    }
}
