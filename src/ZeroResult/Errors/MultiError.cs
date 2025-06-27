using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace ZeroResult.Errors;

/// <summary>
/// Builder for constructing MultiError instances incrementally
/// This allows for efficient accumulation of errors without immediate allocation
/// </summary>
/// <remarks>
/// Initializes a new instance of the Builder with an optional initial capacity
/// This allows for efficient memory allocation when the number of errors is known in advance
/// </remarks>
[method: MethodImpl(MethodImplOptions.AggressiveInlining)]
public readonly ref struct MultiErrorBuilder(int initialCapacity = 4)
{
    private readonly List<IError> _list = new(initialCapacity);

    /// <summary>
    /// Adds a single error to the builder
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Add(IError error)
    {
        _list.Add(error);
    }

    /// <summary>
    /// Adds a range of errors to the builder
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void AddRange(ReadOnlySpan<IError> errors)
    {
        foreach (var error in errors)
            _list.Add(error);
    }

    /// <summary>
    /// Builds the MultiError instance from the accumulated errors
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly MultiError Build()
    {
        return new MultiError(_list.AsReadOnly());
    }
}

/// <summary>
/// Represents a collection of errors that can occur during operations
/// </summary>
public sealed class MultiError : IError
{
    private readonly ReadOnlyCollection<IError> _errors;
    private string? _cachedMessage;

    /// <summary>
    /// Gets the number of errors in this collection
    /// </summary>
    public int Count { get; }

    /// <summary>
    /// Gets a descriptive error message combining all contained errors
    /// </summary>
    public string Message
    {
        get
        {
            return _cachedMessage ??= FormatMessage();
        }
    }

    /// <summary>
    /// Gets an optional error code (returns first error's code)
    /// </summary>
    public string? Code => Count > 0 ? _errors[0].Code : null;

    /// <summary>
    /// Creates a MultiError from existing errors
    /// </summary>
    public MultiError(IError[] errors)
    {
        ArgumentNullException.ThrowIfNull(errors);
        if (errors.Length == 0)
            throw new ArgumentException("Error collection cannot be empty");

        _errors = errors.AsReadOnly();
        Count = errors.Length;
    }

    /// <summary>
    /// Creates a MultiError from an existing ReadOnlyCollection of errors
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MultiError(ReadOnlyCollection<IError> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);
        if (errors.Count == 0)
            throw new ArgumentException("Error collection cannot be empty");

        _errors = errors;
        Count = errors.Count;
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection of errors
    /// </summary>
    public IEnumerable<IError> AsEnumerable()
    {
        return _errors;
    }

    /// <summary>
    /// Gets the error at the specified index
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IError GetErrorAt(int index)
    {
        if ((uint)index >= (uint)Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        return _errors[index];
    }

    private string FormatMessage()
    {
        if (Count == 1)
            return _errors[0].Message;

        var sb = new StringBuilder();
        sb.Append(CultureInfo.InvariantCulture, $"Multiple errors occurred ({Count}):");

        for (int i = 0; i < Count; i++)
        {
            sb.AppendLine();
            sb.Append(CultureInfo.InvariantCulture, $"- {_errors[i].Message}");

            if (string.IsNullOrEmpty(_errors[i].Code) == false)
            {
                sb.Append(CultureInfo.InvariantCulture, $" (Code: {_errors[i].Code})");
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// Creates a builder for incremental construction
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MultiErrorBuilder CreateBuilder(int initialCapacity = 4)
    {
        return new MultiErrorBuilder(initialCapacity);
    }

    /// <summary>
    /// Merges two MultiError instances
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MultiError Merge(MultiError first, MultiError second)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);

        var builder = CreateBuilder(first.Count + second.Count);

        for (int i = 0; i < first.Count; i++)
            builder.Add(first.GetErrorAt(i));

        for (int i = 0; i < second.Count; i++)
            builder.Add(second.GetErrorAt(i));

        return builder.Build();
    }
}
