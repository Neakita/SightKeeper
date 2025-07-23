using System.Buffers;

namespace SightKeeper.Application.ScreenCapturing.Saving;

public sealed class BufferedConvertingImageSaverFactory<TSourcePixel, TTargetPixel>(
	int maxArrayLength,
	int maxArraysPerBucket,
	PixelConverter<TSourcePixel, TTargetPixel> pixelConverter,
	ImageDataSaver<TTargetPixel> nextDataSaver)
	: ImageSaverFactory<TSourcePixel> where TSourcePixel : unmanaged where TTargetPixel : unmanaged
{
	public ImageSaver<TSourcePixel> CreateImageSaver()
	{
		var arrayPool = ArrayPool<TSourcePixel>.Create(maxArrayLength, maxArraysPerBucket);
		return new ImmediateImageSaver<TSourcePixel>
		{
			DataSaver = new BufferedImageDataSaverMiddleware<TSourcePixel>
			{
				Next = new ImageDataConverterMiddleware<TSourcePixel, TTargetPixel>
				{
					Converter = pixelConverter,
					Next = nextDataSaver
				},
				ArrayPool = arrayPool
			}
		};
	}
}