using CommunityToolkit.HighPerformance;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ScreenCapturing.Saving;

public sealed class ImageDataConverterMiddleware<TSourcePixel, TTargetPixel> : ImageDataSaver<TSourcePixel>
{
	public required PixelConverter<TSourcePixel, TTargetPixel> Converter { get; init; }
	public required ImageDataSaver<TTargetPixel> Next { get; init; }

	public void SaveData(Image image, ReadOnlySpan2D<TSourcePixel> data)
	{
		var requiredBufferSize = data.Width * data.Height;
		EnsureBufferCapacity(requiredBufferSize);
		var bufferAsSpan = _buffer.AsSpan().AsSpan2D(data.Height, data.Width);
		Converter.Convert(data, bufferAsSpan);
		Next.SaveData(image, bufferAsSpan);
	}

	private TTargetPixel[] _buffer = Array.Empty<TTargetPixel>();

	private void EnsureBufferCapacity(int requiredBufferSize)
	{
		if (requiredBufferSize > _buffer.Length)
			_buffer = new TTargetPixel[requiredBufferSize];
	}
}