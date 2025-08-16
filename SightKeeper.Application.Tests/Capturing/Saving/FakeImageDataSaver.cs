using CommunityToolkit.HighPerformance;
using SightKeeper.Application.ScreenCapturing.Saving;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Tests.Capturing.Saving;

internal sealed class FakeImageDataSaver<TPixel> : ImageDataSaver<TPixel>
{
	public List<(Image image, TPixel[,] data)> ReceivedCalls { get; } = new();
	public bool HoldCalls { get; set; }

	public void SaveData(Image image, ReadOnlySpan2D<TPixel> data)
	{
		while (HoldCalls)
			Thread.Sleep(1);
		ReceivedCalls.Add((image, data.ToArray()));
	}
}