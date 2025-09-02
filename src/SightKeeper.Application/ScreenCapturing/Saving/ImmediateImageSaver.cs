using CommunityToolkit.HighPerformance;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ScreenCapturing.Saving;

public sealed class ImmediateImageSaver<TPixel> : ImageSaver<TPixel>, LimitedSaver
	where TPixel : unmanaged
{
	public required ImageDataSaver<TPixel> DataSaver { get; init; }

	public bool IsLimitReached => DataSaver is LimitedSaver { IsLimitReached: true };
	public Task Processing => DataSaver is LimitedSaver limitedSaver ? limitedSaver.Processing : Task.CompletedTask;

	public void SaveImage(ImageSet set, ReadOnlySpan2D<TPixel> imageData, DateTimeOffset creationTimestamp)
	{
		var imageSize = new Vector2<ushort>((ushort)imageData.Width, (ushort)imageData.Height);
		var image = set.CreateImage(creationTimestamp, imageSize);
		DataSaver.SaveData(image, imageData);
	}
}