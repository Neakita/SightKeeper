using CommunityToolkit.HighPerformance;
using SightKeeper.Application.ScreenCapturing.Saving;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Tests.Capturing.Saving;

internal sealed class FakeImageSaver<TPixel> : ImageSaver<TPixel>
{
	public bool HoldCalls { get; set; }
	public List<(DomainImageSet set, TPixel[,] imageData, DateTimeOffset creationTimestamp)> ReceivedCalls { get; } = new();

	public void SaveImage(DomainImageSet set, ReadOnlySpan2D<TPixel> imageData, DateTimeOffset creationTimestamp)
	{
		ReceivedCalls.Add((set, imageData.ToArray(), creationTimestamp));
		while (HoldCalls)
			Thread.Sleep(1);
	}
}