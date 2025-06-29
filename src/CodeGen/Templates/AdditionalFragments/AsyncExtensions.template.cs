    /// <summary>
    /// Asynchronously maps the successful value to a new type when the source is a {{TaskType}}-wrapped result.
    /// Preserves the error state if the source result is failed.
    /// </summary>
    /// <typeparam name="T">The type of the source result's success value</typeparam>
    /// <typeparam name="TError">The error type implementing <see cref="IError"/></typeparam>
    /// <typeparam name="TResult">The type of the mapped success value</typeparam>
    /// <param name="source">The source result wrapped in {{TaskType}}</param>
    /// <param name="mapper">The asynchronous mapping function to apply to the success value</param>
    /// <returns>A {{TaskType}} representing the mapped result</returns>
    /// <exception cref="ArgumentNullException">Thrown if mapper is null</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async {{TaskType}}<{{ResultType}}<TResult, TError>> MapAsync<T, TError, TResult>(
        this {{TaskType}}<{{ResultType}}<T, TError>> source,
        Func<T, {{TaskType}}<TResult>> mapper)
        where TError : IError
    {
        var result = await source.ConfigureAwait(false);
        return await result.MapAsync(mapper).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously binds the successful value to a new result when the source is a {{TaskType}}-wrapped result.
    /// Short-circuits if the source result is failed.
    /// </summary>
    /// <typeparam name="T">The type of the source result's success value</typeparam>
    /// <typeparam name="TError">The error type implementing <see cref="IError"/></typeparam>
    /// <typeparam name="TResult">The success type of the bound result</typeparam>
    /// <param name="source">The source result wrapped in {{TaskType}}</param>
    /// <param name="binder">The asynchronous binding function that produces a new result</param>
    /// <returns>A {{TaskType}} representing the bound result</returns>
    /// <exception cref="ArgumentNullException">Thrown if binder is null</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async {{TaskType}}<{{ResultType}}<TResult, TError>> BindAsync<T, TError, TResult>(
        this {{TaskType}}<{{ResultType}}<T, TError>> source,
        Func<T, {{TaskType}}<{{ResultType}}<TResult, TError>>> binder)
        where TError : IError
    {
        var result = await source.ConfigureAwait(false);
        return await result.BindAsync(binder).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously ensures a predicate is satisfied when the source is a {{TaskType}}-wrapped result.
    /// Returns the original error if the source is failed or produces a new error if the predicate fails.
    /// </summary>
    /// <typeparam name="T">The type of the success value</typeparam>
    /// <typeparam name="TError">The error type implementing <see cref="IError"/></typeparam>
    /// <param name="source">The source result wrapped in {{TaskType}}</param>
    /// <param name="predicate">The asynchronous predicate to test the success value</param>
    /// <param name="errorFactory">The asynchronous factory for creating an error if the predicate fails</param>
    /// <returns>A {{TaskType}} representing the validated result</returns>
    /// <exception cref="ArgumentNullException">Thrown if predicate or errorFactory is null</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async {{TaskType}}<{{ResultType}}<T, TError>> EnsureAsync<T, TError>(
        this {{TaskType}}<{{ResultType}}<T, TError>> source,
        Func<T, {{TaskType}}<bool>> predicate,
        Func<{{TaskType}}<TError>> errorFactory)
        where TError : IError
    {
        var result = await source.ConfigureAwait(false);
        return await result.EnsureAsync(predicate, errorFactory).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously executes an action on success when the source is a {{TaskType}}-wrapped result.
    /// Does nothing if the source result is failed.
    /// </summary>
    /// <typeparam name="T">The type of the success value</typeparam>
    /// <typeparam name="TError">The error type implementing <see cref="IError"/></typeparam>
    /// <param name="source">The source result wrapped in {{TaskType}}</param>
    /// <param name="action">The asynchronous action to execute on success</param>
    /// <returns>A {{TaskType}} representing the original result</returns>
    /// <exception cref="ArgumentNullException">Thrown if action is null</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async {{TaskType}}<{{ResultType}}<T, TError>> OnSuccessAsync<T, TError>(
        this {{TaskType}}<{{ResultType}}<T, TError>> source,
        Func<T, {{TaskType}}> action)
        where TError : IError
    {
        var result = await source.ConfigureAwait(false);
        return await result.OnSuccessAsync(action).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously executes an action on failure when the source is a {{TaskType}}-wrapped result.
    /// Does nothing if the source result is successful.
    /// </summary>
    /// <typeparam name="T">The type of the success value</typeparam>
    /// <typeparam name="TError">The error type implementing <see cref="IError"/></typeparam>
    /// <param name="source">The source result wrapped in {{TaskType}}</param>
    /// <param name="action">The asynchronous action to execute on failure</param>
    /// <returns>A {{TaskType}} representing the original result</returns>
    /// <exception cref="ArgumentNullException">Thrown if action is null</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async {{TaskType}}<{{ResultType}}<T, TError>> OnFailureAsync<T, TError>(
        this {{TaskType}}<{{ResultType}}<T, TError>> source,
        Func<TError, {{TaskType}}> action)
        where TError : IError
    {
        var result = await source.ConfigureAwait(false);
        return await result.OnFailureAsync(action).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously matches and transforms the result when the source is a {{TaskType}}-wrapped result.
    /// Executes the appropriate transformation function based on success/failure state.
    /// </summary>
    /// <typeparam name="T">The type of the success value</typeparam>
    /// <typeparam name="TError">The error type implementing <see cref="IError"/></typeparam>
    /// <typeparam name="TResult">The type of the transformed result</typeparam>
    /// <param name="source">The source result wrapped in {{TaskType}}</param>
    /// <param name="onSuccess">The asynchronous function to transform a success value</param>
    /// <param name="onFailure">The asynchronous function to transform an error</param>
    /// <returns>A {{TaskType}} representing the transformed result</returns>
    /// <exception cref="ArgumentNullException">Thrown if onSuccess or onFailure is null</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async {{TaskType}}<TResult> MatchAsync<T, TError, TResult>(
        this {{TaskType}}<{{ResultType}}<T, TError>> source,
        Func<T, {{TaskType}}<TResult>> onSuccess,
        Func<TError, {{TaskType}}<TResult>> onFailure)
        where TError : IError
    {
        var result = await source.ConfigureAwait(false);
        return await result.MatchAsync(onSuccess, onFailure).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously matches and executes side effects based on the result state when the source is a {{TaskType}}-wrapped result.
    /// Executes the appropriate action without modifying the result.
    /// </summary>
    /// <typeparam name="T">The type of the success value</typeparam>
    /// <typeparam name="TError">The error type implementing <see cref="IError"/></typeparam>
    /// <param name="source">The source result wrapped in {{TaskType}}</param>
    /// <param name="onSuccess">The asynchronous action to execute on success</param>
    /// <param name="onFailure">The asynchronous action to execute on failure</param>
    /// <returns>A {{TaskType}} representing the completion of the matched action</returns>
    /// <exception cref="ArgumentNullException">Thrown if onSuccess or onFailure is null</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async {{TaskType}} MatchAsync<T, TError>(
        this {{TaskType}}<{{ResultType}}<T, TError>> source,
        Func<T, {{TaskType}}> onSuccess,
        Func<TError, {{TaskType}}> onFailure)
        where TError : IError
    {
        var result = await source.ConfigureAwait(false);
        await result.MatchAsync(onSuccess, onFailure).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously taps into a successful result without modifying it when the source is a {{TaskType}}-wrapped result.
    /// Executes the action if the result is successful and returns the original result.
    /// </summary>
    /// <typeparam name="T">The type of the success value</typeparam>
    /// <typeparam name="TError">The error type implementing <see cref="IError"/></typeparam>
    /// <param name="source">The source result wrapped in {{TaskType}}</param>
    /// <param name="action">The asynchronous action to execute on success</param>
    /// <returns>A {{TaskType}} representing the original result</returns>
    /// <exception cref="ArgumentNullException">Thrown if action is null</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async {{TaskType}}<{{ResultType}}<T, TError>> TapAsync<T, TError>(
        this {{TaskType}}<{{ResultType}}<T, TError>> source,
        Func<T, {{TaskType}}> action)
        where TError : IError
    {
        var result = await source.ConfigureAwait(false);
        await result.OnSuccessAsync(action).ConfigureAwait(false);
        return result;
    }