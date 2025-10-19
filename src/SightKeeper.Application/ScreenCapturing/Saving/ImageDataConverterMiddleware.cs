using CommunityToolkit.HighPerformance;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ScreenCapturing.Saving;

internal sealed class ImageDataConverterMiddleware<TSourcePixel, TTargetPixel>(
	ImageDataSaver<TTargetPixel> next,
	PixelConverter<TSourcePixel, TTargetPixel> converter)
	: ImageDataSaver<TSourcePixel>
{
	public void SaveData(ManagedImage image, ReadOnlySpan2D<TSourcePixel> data)
	{
		var requiredBufferSize = data.Width * data.Height;
		EnsureBufferCapacity(requiredBufferSize);
		var bufferAsSpan = _buffer.AsSpan().AsSpan2D(data.Height, data.Width);
		converter.Convert(data, bufferAsSpan);
		next.SaveData(image, bufferAsSpan);
	}

	private TTargetPixel[] _buffer = Array.Empty<TTargetPixel>();

	private void EnsureBufferCapacity(int requiredBufferSize)
	{
		if (requiredBufferSize > _buffer.Length)
			_buffer = new TTargetPixel[requiredBufferSize];
	}
}