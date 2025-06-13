using System.Diagnostics;
using ZeroResult.Core.Errors;

namespace ZeroResult.Core.Models;

/// <summary>
/// A heap-allocated Result monad for high-performance scenarios that represents either
/// a successful value of type <typeparamref name="T"/> or an error of type <typeparamref name="TError"/>.
/// </summary>
/// <typeparam name="T">The type of the successful result value</typeparam>
/// <typeparam name="TError">The type of the error value, must implement <see cref="IError"/></typeparam>
[DebuggerDisplay("IsSuccess = {IsSuccess}, Value = {(_isSuccess ? _value : default)}, Error = {(_isSuccess ? default : _error)}")]
public readonly partial struct HeapResult<T, TError> : IEquatable<HeapResult<T, TError>>
    where TError : IError
{
    /// <summary>
    /// Determines whether the specified object is equal to the current instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns>true if the specified object is equal to the current instance; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        return obj is HeapResult<T, TError> other && Equals(other);
    }

    /// <summary>
    /// Determines whether two <see cref="HeapResult{T, TError}"/> instances are equal.
    /// </summary>
    /// <param name="other">The instance to compare with the current instance.</param>
    /// <returns>true if the instances are equal; otherwise, false.</returns>
    public bool Equals(HeapResult<T, TError> other)
    {
        if (_isSuccess != other._isSuccess)
            return false;

        return _isSuccess 
            ? EqualityComparer<T>.Default.Equals(_value, other._value)
            : EqualityComparer<TError>.Default.Equals(_error, other._error);
    }

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode()
    {
        return _isSuccess
            ? HashCode.Combine(true, _value)
            : HashCode.Combine(false, _error);
    }

    /// <summary>
    /// Determines whether two specified <see cref="HeapResult{T, TError}"/> instances are equal.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>true if the instances are equal; otherwise, false.</returns>
    public static bool operator ==(HeapResult<T, TError> left, HeapResult<T, TError> right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two specified <see cref="HeapResult{T, TError}"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>true if the instances are not equal; otherwise, false.</returns>
    public static bool operator !=(HeapResult<T, TError> left, HeapResult<T, TError> right)
    {
        return !(left == right);
    }
}
