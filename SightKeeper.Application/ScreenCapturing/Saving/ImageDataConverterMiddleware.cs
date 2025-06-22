using System.Buffers;
using CommunityToolkit.HighPerformance;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ScreenCapturing.Saving;

public sealed class ImageDataConverterMiddleware<TSourcePixel, TTargetPixel> : ImageDataSaver<TSourcePixel>
	where TSourcePixel : unmanaged
	where TTargetPixel : unmanaged
{
	public required PixelConverter<TSourcePixel, TTargetPixel> Converter { get; init; }
	public required ImageDataSaver<TTargetPixel> Next { get; init; }

	public void SaveData(Image image, ReadOnlySpan2D<TSourcePixel> data)
	{
		var bufferSize = data.Width * data.Height;
		using var bufferOwner = MemoryPool<TTargetPixel>.Shared.Rent(bufferSize);
		var convertedData = bufferOwner.Memory.AsMemory2D(data.Height, data.Width).Span;
		Converter.Convert(data, convertedData);
		Next.SaveData(image, convertedData);
	}
}