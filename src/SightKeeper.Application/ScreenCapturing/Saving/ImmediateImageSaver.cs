using CommunityToolkit.HighPerformance;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ScreenCapturing.Saving;

internal sealed class ImmediateImageSaver<TPixel>(ImageDataSaver<TPixel> dataSaver) : ImageSaver<TPixel>, LimitedSaver
	where TPixel : unmanaged
{
	public bool IsLimitReached => dataSaver is LimitedSaver { IsLimitReached: true };
	public Task Processing => dataSaver is LimitedSaver limitedSaver ? limitedSaver.Processing : Task.CompletedTask;

	public void SaveImage(ImageSet set, ReadOnlySpan2D<TPixel> imageData, DateTimeOffset creationTimestamp)
	{
		var imageSize = new Vector2<ushort>((ushort)imageData.Width, (ushort)imageData.Height);
		var image = set.CreateImage(creationTimestamp, imageSize);
		dataSaver.SaveData(image, imageData);
	}
}