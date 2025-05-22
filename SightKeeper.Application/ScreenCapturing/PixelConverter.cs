using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;

namespace SightKeeper.Application.ScreenCapturing;

public abstract class PixelConverter<TFrom, TTo>
{
	public void Convert(ReadOnlySpan2D<TFrom> source, Span2D<TTo> target)
	{
		if (source.TryGetSpan(out var contiguousSource) && target.TryGetSpan(out var contiguousTarget))
		{
			Convert(contiguousSource, contiguousTarget);
			return;
		}
		Guard.IsEqualTo(source.Height, target.Height);
		for (int i = 0; i < source.Height; i++)
		{
			var sourceRow = source.GetRowSpan(i);
			var targetRow = target.GetRowSpan(i);
			Convert(sourceRow, targetRow);
		}
	}

	public abstract void Convert(ReadOnlySpan<TFrom> source, Span<TTo> target);
}