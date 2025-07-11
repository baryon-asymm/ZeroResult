//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by CodeGen v1.0.0
//     Script: generate-code.bat (or generate-code.sh for Unix)
//     Timestamp: 6/29/2025
//     Template: Data.template.cs
//     Target: StackResult (net8.0+)
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     regenerated. Consider modifying the template instead.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable enable

using System.Runtime.CompilerServices;
using ZeroResult.Errors;

namespace ZeroResult;

public readonly ref partial struct StackResult<T, TError>
    where TError : IError
{
    private readonly T? _value;
    private readonly TError? _error;
    private readonly bool _isSuccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="StackResult{T, TError}"/> struct with a successful value.
    /// </summary>
    /// <param name="value">The successful value of type <typeparamref name="T"/>.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal StackResult(T value)
    {
        _value = value;
        _error = default;
        _isSuccess = true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StackResult{T, TError}"/> struct with an error.
    /// </summary>
    /// <param name="error">The error of type <typeparamref name="TError"/>.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal StackResult(TError error)
    {
        _value = default;
        _error = error;
        _isSuccess = false;
    }

    /// <summary>
    /// Indicates whether the result is successful.
    /// </summary>
    public bool IsSuccess
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _isSuccess;
    }

    /// <summary>
    /// Indicates whether the result is a failure.
    /// </summary>
    public bool IsFailure
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => !_isSuccess;
    }
}
