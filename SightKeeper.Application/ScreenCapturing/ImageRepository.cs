using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using SightKeeper.Application.ScreenCapturing.Saving;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.ScreenCapturing;

public class ImageRepository : ImageSaver<Rgba32>
{
	public ImageRepository()
	{
	}

	public void SaveImage(ImageSet set, ReadOnlySpan2D<Rgba32> imageData, DateTimeOffset creationTimestamp)
	{
		Vector2<ushort> resolution = new((ushort)imageData.Width, (ushort)imageData.Height);
		var image = set.CreateImage(creationTimestamp, resolution);
		using var stream = image.OpenWriteStream();
		Guard.IsNotNull(stream);
		if (imageData.TryGetSpan(out var contiguousSpan))
		{
			stream.Write(contiguousSpan.AsBytes());
			return;
		}
		for (int i = 0; i < imageData.Height; i++)
		{
			var rowSpan = imageData.GetRowSpan(i);
			stream.Write(rowSpan.AsBytes());
		}
	}
}