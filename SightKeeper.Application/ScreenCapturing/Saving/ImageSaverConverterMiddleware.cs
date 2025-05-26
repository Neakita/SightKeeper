using System.Buffers;
using CommunityToolkit.HighPerformance;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ScreenCapturing.Saving;

public sealed class ImageSaverConverterMiddleware<TSourcePixel, TTargetPixel> : ImageSaver<TSourcePixel>
{
	public required PixelConverter<TSourcePixel, TTargetPixel> Converter { private get; init; }
	public required ImageSaver<TTargetPixel> NextMiddleware { private get; init; }

	public void SaveImage(ImageSet set, ReadOnlySpan2D<TSourcePixel> imageData, DateTimeOffset creationTimestamp)
	{
		var bufferLength = imageData.Width * imageData.Height;
		using var bufferOwner = MemoryPool<TTargetPixel>.Shared.Rent(bufferLength);
		var bufferSpan = bufferOwner.Memory.AsMemory2D(imageData.Height, imageData.Width).Span;
		Converter.Convert(imageData, bufferSpan);
		NextMiddleware.SaveImage(set, bufferSpan, creationTimestamp);
	}
}