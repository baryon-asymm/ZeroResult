using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ZeroResult.Core.Helpers;

internal static class ThrowHelper
{
    /// <summary>
    /// Throws an <see cref="InvalidOperationException"/> indicating that an invalid access was attempted.
    /// </summary>
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowInvalidAccess(string message) =>
        throw new InvalidOperationException(message);
}
