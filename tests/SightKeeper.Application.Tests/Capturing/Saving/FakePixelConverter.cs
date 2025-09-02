using CommunityToolkit.HighPerformance;
using SightKeeper.Application.ScreenCapturing;

namespace SightKeeper.Application.Tests.Capturing.Saving;

internal sealed class FakePixelConverter<TSourcePixel, TTargetPixel> : PixelConverter<TSourcePixel, TTargetPixel>
{
	public List<TSourcePixel[,]> ReceivedCalls { get; } = new();

	public FakePixelConverter(Memory2D<TTargetPixel> fakeTargetContent)
	{
		_fakeTargetContent = fakeTargetContent;
	}

	public override void Convert(ReadOnlySpan2D<TSourcePixel> source, Span2D<TTargetPixel> target)
	{
		ReceivedCalls.Add(source.ToArray());
		_fakeTargetContent.Span.CopyTo(target);
	}

	public override void Convert(ReadOnlySpan<TSourcePixel> source, Span<TTargetPixel> target)
	{
		_fakeTargetContent.Span.CopyTo(target);
	}

	private readonly Memory2D<TTargetPixel> _fakeTargetContent;
}