//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by CodeGen v1.0.0
//     Script: generate-code.bat (or generate-code.sh for Unix)
//     Timestamp: 6/29/2025
//     Template: Conversions.template.cs
//     Target: Result (net8.0+)
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     regenerated. Consider modifying the template instead.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using ZeroResult.Errors;

namespace ZeroResult;

public readonly partial struct Result<T, TError>
    where TError : IError
{
    /// <summary>
    /// Implicitly converts a value to a successful <see cref="Result{T, TError}"/>.
    /// </summary>
    /// <param name="value">The success value to convert</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Result<T, TError>(T value)
        => new(value);

    /// <summary>
    /// Implicitly converts an error to a failed <see cref="Result{T, TError}"/>.
    /// </summary>
    /// <param name="error">The error to convert</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Result<T, TError>(TError error)
        => new(error);

    /// <summary>
    /// Implicitly converts a <see cref="Result{T, TError}"/> to a completed <see cref="ValueTask"/> of <see cref="Result{T, TError}"/>.
    /// </summary>
    /// <param name="result">The result to convert</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ValueTask<Result<T, TError>>(Result<T, TError> result)
        => new(result);
    
    /// <summary>
    /// Implicitly converts a <see cref="Result{T, TError}"/> to a completed <see cref="Task"/> of <see cref="Result{T, TError}"/>.
    /// </summary>
    /// <param name="result">The result to convert</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Task<Result<T, TError>>(Result<T, TError> result)
        => Task.FromResult(result);

    /// <summary>
    /// Converts the current <see cref="Result{T, TError}"/> to <see cref="StackResult{T, TError}"/>.
    /// </summary>
    /// <returns>
    /// A new <see cref="StackResult{T, TError}"/> with the same success/failure state.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public StackResult<T, TError> AsStackResult() 
        => IsSuccess 
            ? new StackResult<T, TError>(_value) 
            : new StackResult<T, TError>(_error);
}
