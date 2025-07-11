//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by CodeGen v1.0.0
//     Script: generate-code.bat (or generate-code.sh for Unix)
//     Timestamp: 6/29/2025
//     Template: AsyncOperations.Extensions.template.cs
//     Target: Result (net8.0+)
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     regenerated. Consider modifying the template instead.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Diagnostics;
using System.Runtime.CompilerServices;
using ZeroResult.Errors;

namespace ZeroResult;

/// <summary>
/// Provides extension methods for asynchronous operations on <see cref="Result{T, TError}"/> wrapped in ValueTask.
/// Enables fluent asynchronous result chaining.
/// </summary>
public static partial class ResultExtensions
{
    /// <summary>
    /// Asynchronously maps the successful value to a new type when the source is a ValueTask-wrapped result.
    /// Preserves the error state if the source result is failed.
    /// </summary>
    /// <typeparam name="T">The type of the source result's success value</typeparam>
    /// <typeparam name="TError">The error type implementing <see cref="IError"/></typeparam>
    /// <typeparam name="TResult">The type of the mapped success value</typeparam>
    /// <param name="source">The source result wrapped in ValueTask</param>
    /// <param name="mapper">The asynchronous mapping function to apply to the success value</param>
    /// <returns>A ValueTask representing the mapped result</returns>
    /// <exception cref="ArgumentNullException">Thrown if mapper is null</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async ValueTask<Result<TResult, TError>> MapAsync<T, TError, TResult>(
        this ValueTask<Result<T, TError>> source,
        Func<T, ValueTask<TResult>> mapper)
        where TError : IError
    {
        var result = await source.ConfigureAwait(false);
        return await result.MapAsync(mapper).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously binds the successful value to a new result when the source is a ValueTask-wrapped result.
    /// Short-circuits if the source result is failed.
    /// </summary>
    /// <typeparam name="T">The type of the source result's success value</typeparam>
    /// <typeparam name="TError">The error type implementing <see cref="IError"/></typeparam>
    /// <typeparam name="TResult">The success type of the bound result</typeparam>
    /// <param name="source">The source result wrapped in ValueTask</param>
    /// <param name="binder">The asynchronous binding function that produces a new result</param>
    /// <returns>A ValueTask representing the bound result</returns>
    /// <exception cref="ArgumentNullException">Thrown if binder is null</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async ValueTask<Result<TResult, TError>> BindAsync<T, TError, TResult>(
        this ValueTask<Result<T, TError>> source,
        Func<T, ValueTask<Result<TResult, TError>>> binder)
        where TError : IError
    {
        var result = await source.ConfigureAwait(false);
        return await result.BindAsync(binder).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously ensures a predicate is satisfied when the source is a ValueTask-wrapped result.
    /// Returns the original error if the source is failed or produces a new error if the predicate fails.
    /// </summary>
    /// <typeparam name="T">The type of the success value</typeparam>
    /// <typeparam name="TError">The error type implementing <see cref="IError"/></typeparam>
    /// <param name="source">The source result wrapped in ValueTask</param>
    /// <param name="predicate">The asynchronous predicate to test the success value</param>
    /// <param name="errorFactory">The asynchronous factory for creating an error if the predicate fails</param>
    /// <returns>A ValueTask representing the validated result</returns>
    /// <exception cref="ArgumentNullException">Thrown if predicate or errorFactory is null</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async ValueTask<Result<T, TError>> EnsureAsync<T, TError>(
        this ValueTask<Result<T, TError>> source,
        Func<T, ValueTask<bool>> predicate,
        Func<ValueTask<TError>> errorFactory)
        where TError : IError
    {
        var result = await source.ConfigureAwait(false);
        return await result.EnsureAsync(predicate, errorFactory).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously executes an action on success when the source is a ValueTask-wrapped result.
    /// Does nothing if the source result is failed.
    /// </summary>
    /// <typeparam name="T">The type of the success value</typeparam>
    /// <typeparam name="TError">The error type implementing <see cref="IError"/></typeparam>
    /// <param name="source">The source result wrapped in ValueTask</param>
    /// <param name="action">The asynchronous action to execute on success</param>
    /// <returns>A ValueTask representing the original result</returns>
    /// <exception cref="ArgumentNullException">Thrown if action is null</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async ValueTask<Result<T, TError>> OnSuccessAsync<T, TError>(
        this ValueTask<Result<T, TError>> source,
        Func<T, ValueTask> action)
        where TError : IError
    {
        var result = await source.ConfigureAwait(false);
        return await result.OnSuccessAsync(action).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously executes an action on failure when the source is a ValueTask-wrapped result.
    /// Does nothing if the source result is successful.
    /// </summary>
    /// <typeparam name="T">The type of the success value</typeparam>
    /// <typeparam name="TError">The error type implementing <see cref="IError"/></typeparam>
    /// <param name="source">The source result wrapped in ValueTask</param>
    /// <param name="action">The asynchronous action to execute on failure</param>
    /// <returns>A ValueTask representing the original result</returns>
    /// <exception cref="ArgumentNullException">Thrown if action is null</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async ValueTask<Result<T, TError>> OnFailureAsync<T, TError>(
        this ValueTask<Result<T, TError>> source,
        Func<TError, ValueTask> action)
        where TError : IError
    {
        var result = await source.ConfigureAwait(false);
        return await result.OnFailureAsync(action).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously matches and transforms the result when the source is a ValueTask-wrapped result.
    /// Executes the appropriate transformation function based on success/failure state.
    /// </summary>
    /// <typeparam name="T">The type of the success value</typeparam>
    /// <typeparam name="TError">The error type implementing <see cref="IError"/></typeparam>
    /// <typeparam name="TResult">The type of the transformed result</typeparam>
    /// <param name="source">The source result wrapped in ValueTask</param>
    /// <param name="onSuccess">The asynchronous function to transform a success value</param>
    /// <param name="onFailure">The asynchronous function to transform an error</param>
    /// <returns>A ValueTask representing the transformed result</returns>
    /// <exception cref="ArgumentNullException">Thrown if onSuccess or onFailure is null</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async ValueTask<TResult> MatchAsync<T, TError, TResult>(
        this ValueTask<Result<T, TError>> source,
        Func<T, ValueTask<TResult>> onSuccess,
        Func<TError, ValueTask<TResult>> onFailure)
        where TError : IError
    {
        var result = await source.ConfigureAwait(false);
        return await result.MatchAsync(onSuccess, onFailure).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously matches and executes side effects based on the result state when the source is a ValueTask-wrapped result.
    /// Executes the appropriate action without modifying the result.
    /// </summary>
    /// <typeparam name="T">The type of the success value</typeparam>
    /// <typeparam name="TError">The error type implementing <see cref="IError"/></typeparam>
    /// <param name="source">The source result wrapped in ValueTask</param>
    /// <param name="onSuccess">The asynchronous action to execute on success</param>
    /// <param name="onFailure">The asynchronous action to execute on failure</param>
    /// <returns>A ValueTask representing the completion of the matched action</returns>
    /// <exception cref="ArgumentNullException">Thrown if onSuccess or onFailure is null</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async ValueTask MatchAsync<T, TError>(
        this ValueTask<Result<T, TError>> source,
        Func<T, ValueTask> onSuccess,
        Func<TError, ValueTask> onFailure)
        where TError : IError
    {
        var result = await source.ConfigureAwait(false);
        await result.MatchAsync(onSuccess, onFailure).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously taps into a successful result without modifying it when the source is a ValueTask-wrapped result.
    /// Executes the action if the result is successful and returns the original result.
    /// </summary>
    /// <typeparam name="T">The type of the success value</typeparam>
    /// <typeparam name="TError">The error type implementing <see cref="IError"/></typeparam>
    /// <param name="source">The source result wrapped in ValueTask</param>
    /// <param name="action">The asynchronous action to execute on success</param>
    /// <returns>A ValueTask representing the original result</returns>
    /// <exception cref="ArgumentNullException">Thrown if action is null</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async ValueTask<Result<T, TError>> TapAsync<T, TError>(
        this ValueTask<Result<T, TError>> source,
        Func<T, ValueTask> action)
        where TError : IError
    {
        var result = await source.ConfigureAwait(false);
        await result.OnSuccessAsync(action).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Asynchronously maps the successful value to a new type when the source is a Task-wrapped result.
    /// Preserves the error state if the source result is failed.
    /// </summary>
    /// <typeparam name="T">The type of the source result's success value</typeparam>
    /// <typeparam name="TError">The error type implementing <see cref="IError"/></typeparam>
    /// <typeparam name="TResult">The type of the mapped success value</typeparam>
    /// <param name="source">The source result wrapped in Task</param>
    /// <param name="mapper">The asynchronous mapping function to apply to the success value</param>
    /// <returns>A Task representing the mapped result</returns>
    /// <exception cref="ArgumentNullException">Thrown if mapper is null</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<TResult, TError>> MapAsync<T, TError, TResult>(
        this Task<Result<T, TError>> source,
        Func<T, Task<TResult>> mapper)
        where TError : IError
    {
        var result = await source.ConfigureAwait(false);
        return await result.MapAsync(mapper).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously binds the successful value to a new result when the source is a Task-wrapped result.
    /// Short-circuits if the source result is failed.
    /// </summary>
    /// <typeparam name="T">The type of the source result's success value</typeparam>
    /// <typeparam name="TError">The error type implementing <see cref="IError"/></typeparam>
    /// <typeparam name="TResult">The success type of the bound result</typeparam>
    /// <param name="source">The source result wrapped in Task</param>
    /// <param name="binder">The asynchronous binding function that produces a new result</param>
    /// <returns>A Task representing the bound result</returns>
    /// <exception cref="ArgumentNullException">Thrown if binder is null</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<TResult, TError>> BindAsync<T, TError, TResult>(
        this Task<Result<T, TError>> source,
        Func<T, Task<Result<TResult, TError>>> binder)
        where TError : IError
    {
        var result = await source.ConfigureAwait(false);
        return await result.BindAsync(binder).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously ensures a predicate is satisfied when the source is a Task-wrapped result.
    /// Returns the original error if the source is failed or produces a new error if the predicate fails.
    /// </summary>
    /// <typeparam name="T">The type of the success value</typeparam>
    /// <typeparam name="TError">The error type implementing <see cref="IError"/></typeparam>
    /// <param name="source">The source result wrapped in Task</param>
    /// <param name="predicate">The asynchronous predicate to test the success value</param>
    /// <param name="errorFactory">The asynchronous factory for creating an error if the predicate fails</param>
    /// <returns>A Task representing the validated result</returns>
    /// <exception cref="ArgumentNullException">Thrown if predicate or errorFactory is null</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<T, TError>> EnsureAsync<T, TError>(
        this Task<Result<T, TError>> source,
        Func<T, Task<bool>> predicate,
        Func<Task<TError>> errorFactory)
        where TError : IError
    {
        var result = await source.ConfigureAwait(false);
        return await result.EnsureAsync(predicate, errorFactory).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously executes an action on success when the source is a Task-wrapped result.
    /// Does nothing if the source result is failed.
    /// </summary>
    /// <typeparam name="T">The type of the success value</typeparam>
    /// <typeparam name="TError">The error type implementing <see cref="IError"/></typeparam>
    /// <param name="source">The source result wrapped in Task</param>
    /// <param name="action">The asynchronous action to execute on success</param>
    /// <returns>A Task representing the original result</returns>
    /// <exception cref="ArgumentNullException">Thrown if action is null</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<T, TError>> OnSuccessAsync<T, TError>(
        this Task<Result<T, TError>> source,
        Func<T, Task> action)
        where TError : IError
    {
        var result = await source.ConfigureAwait(false);
        return await result.OnSuccessAsync(action).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously executes an action on failure when the source is a Task-wrapped result.
    /// Does nothing if the source result is successful.
    /// </summary>
    /// <typeparam name="T">The type of the success value</typeparam>
    /// <typeparam name="TError">The error type implementing <see cref="IError"/></typeparam>
    /// <param name="source">The source result wrapped in Task</param>
    /// <param name="action">The asynchronous action to execute on failure</param>
    /// <returns>A Task representing the original result</returns>
    /// <exception cref="ArgumentNullException">Thrown if action is null</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<T, TError>> OnFailureAsync<T, TError>(
        this Task<Result<T, TError>> source,
        Func<TError, Task> action)
        where TError : IError
    {
        var result = await source.ConfigureAwait(false);
        return await result.OnFailureAsync(action).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously matches and transforms the result when the source is a Task-wrapped result.
    /// Executes the appropriate transformation function based on success/failure state.
    /// </summary>
    /// <typeparam name="T">The type of the success value</typeparam>
    /// <typeparam name="TError">The error type implementing <see cref="IError"/></typeparam>
    /// <typeparam name="TResult">The type of the transformed result</typeparam>
    /// <param name="source">The source result wrapped in Task</param>
    /// <param name="onSuccess">The asynchronous function to transform a success value</param>
    /// <param name="onFailure">The asynchronous function to transform an error</param>
    /// <returns>A Task representing the transformed result</returns>
    /// <exception cref="ArgumentNullException">Thrown if onSuccess or onFailure is null</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<TResult> MatchAsync<T, TError, TResult>(
        this Task<Result<T, TError>> source,
        Func<T, Task<TResult>> onSuccess,
        Func<TError, Task<TResult>> onFailure)
        where TError : IError
    {
        var result = await source.ConfigureAwait(false);
        return await result.MatchAsync(onSuccess, onFailure).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously matches and executes side effects based on the result state when the source is a Task-wrapped result.
    /// Executes the appropriate action without modifying the result.
    /// </summary>
    /// <typeparam name="T">The type of the success value</typeparam>
    /// <typeparam name="TError">The error type implementing <see cref="IError"/></typeparam>
    /// <param name="source">The source result wrapped in Task</param>
    /// <param name="onSuccess">The asynchronous action to execute on success</param>
    /// <param name="onFailure">The asynchronous action to execute on failure</param>
    /// <returns>A Task representing the completion of the matched action</returns>
    /// <exception cref="ArgumentNullException">Thrown if onSuccess or onFailure is null</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task MatchAsync<T, TError>(
        this Task<Result<T, TError>> source,
        Func<T, Task> onSuccess,
        Func<TError, Task> onFailure)
        where TError : IError
    {
        var result = await source.ConfigureAwait(false);
        await result.MatchAsync(onSuccess, onFailure).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously taps into a successful result without modifying it when the source is a Task-wrapped result.
    /// Executes the action if the result is successful and returns the original result.
    /// </summary>
    /// <typeparam name="T">The type of the success value</typeparam>
    /// <typeparam name="TError">The error type implementing <see cref="IError"/></typeparam>
    /// <param name="source">The source result wrapped in Task</param>
    /// <param name="action">The asynchronous action to execute on success</param>
    /// <returns>A Task representing the original result</returns>
    /// <exception cref="ArgumentNullException">Thrown if action is null</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<T, TError>> TapAsync<T, TError>(
        this Task<Result<T, TError>> source,
        Func<T, Task> action)
        where TError : IError
    {
        var result = await source.ConfigureAwait(false);
        await result.OnSuccessAsync(action).ConfigureAwait(false);
        return result;
    }
}
