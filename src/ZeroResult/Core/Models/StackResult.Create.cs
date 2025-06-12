using System.Runtime.CompilerServices;
using ZeroResult.Core.Errors;

namespace ZeroResult.Core.Models;

/// <summary>
/// Provides factory methods for creating <see cref="StackResult{T, TError}"/> instances.
/// Designed for zero-allocation result creation in high-performance scenarios.
/// </summary>
/// <remarks>
/// All methods are aggressively inlined and use pass-by-reference semantics (<see langword="in"/>) 
/// to minimize copying of value types.
/// </remarks>
public static partial class StackResult
{
    /// <summary>
    /// Creates a successful <see cref="StackResult{T, TError}"/> with the specified value.
    /// </summary>
    /// <typeparam name="T">The type of the successful result value</typeparam>
    /// <typeparam name="TError">The error type implementing <see cref="IError"/></typeparam>
    /// <param name="value">The success value (passed by readonly reference)</param>
    /// <returns>A successful result containing the value</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StackResult<T, TError> Success<T, TError>(in T value)
        where TError : IError
        => new(value);

    /// <summary>
    /// Creates a failed <see cref="StackResult{T, TError}"/> with the specified error.
    /// </summary>
    /// <typeparam name="T">The expected value type when successful</typeparam>
    /// <typeparam name="TError">The error type implementing <see cref="IError"/></typeparam>
    /// <param name="error">The error value (passed by readonly reference)</param>
    /// <returns>A failed result containing the error</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StackResult<T, TError> Failure<T, TError>(in TError error)
        where TError : IError
        => new(error);
}
