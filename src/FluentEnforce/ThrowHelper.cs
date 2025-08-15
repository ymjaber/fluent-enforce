using System;
using System.Runtime.CompilerServices;

namespace FluentEnforce;

internal static class ThrowHelper
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ThrowArgumentException(string? paramName, string defaultMessage, string? messageOverride = null)
	{
		throw new ArgumentException(messageOverride ?? defaultMessage, paramName);
	}
}


