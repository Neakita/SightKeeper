using System;
using System.Buffers;

namespace SightKeeper.Avalonia.Extensions;

internal static class Extensions
{
	public static void Scroll<T>(this Span<T> span, int indexes)
	{
		var buffer = ArrayPool<T>.Shared.Rent(span.Length);
		span.CopyTo(buffer);
		for (int i = 0; i < 4; i++)
		{
			var scrolledIndex = (i + indexes) % 4;
			if (scrolledIndex < 0)
				scrolledIndex += 4;
			span[i] = buffer[scrolledIndex];
		}
		ArrayPool<T>.Shared.Return(buffer);
	}
}