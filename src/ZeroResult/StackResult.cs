using System.Diagnostics;
using ZeroResult.Errors;

namespace ZeroResult;

/// <summary>
/// A stack-allocated Result monad for high-performance scenarios that represents either 
/// a successful value of type <typeparamref name="T"/> or an error of type <typeparamref name="TError"/>.
/// </summary>
/// <typeparam name="T">The type of the successful result value</typeparam>
/// <typeparam name="TError">The type of the error value, must implement <see cref="IError"/></typeparam>
[DebuggerDisplay("IsSuccess = {IsSuccess}, Value = {(_isSuccess ? _value : default)}, Error = {(_isSuccess ? default : _error)}")]
public readonly ref partial struct StackResult<T, TError>
    where TError : IError;
