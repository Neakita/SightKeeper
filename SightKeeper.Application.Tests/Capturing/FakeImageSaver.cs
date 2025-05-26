using CommunityToolkit.HighPerformance;
using SightKeeper.Application.ScreenCapturing.Saving;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Tests.Capturing;

internal sealed class FakeImageSaver<TPixel> : ImageSaver<TPixel>
{
	public List<(ImageSet set, TPixel[,] imageData)> SaveImageCalls { get; } = new();

	public void SaveImage(ImageSet set, ReadOnlySpan2D<TPixel> imageData, DateTimeOffset creationTimestamp)
	{
		SaveImageCalls.Add((set, imageData.ToArray()));
	}
}