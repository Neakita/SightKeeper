using System.Buffers;
using CommunityToolkit.HighPerformance;
using SightKeeper.Domain;

namespace SightKeeper.Application.Screenshotting.Saving;

internal sealed class ScreenshotData<TPixel> : IDisposable
{
	public DateTimeOffset CreationDate { get; }
	public Vector2<ushort> ImageSize { get; }
	public ReadOnlySpan<TPixel> ImageData => _imageData.AsSpan()[..ImageDataLength];
	public ReadOnlySpan2D<TPixel> ImageData2D => ImageData.AsSpan2D(ImageSize.Y, ImageSize.X);

	public ScreenshotData(
		DateTimeOffset creationDate,
		ReadOnlySpan2D<TPixel> imageData,
		ArrayPool<TPixel> arrayPool)
	{
		_arrayPool = arrayPool;
		CreationDate = creationDate;
		ImageSize = new Vector2<ushort>((ushort)imageData.Width, (ushort)imageData.Height);
		_imageData = arrayPool.Rent(ImageDataLength);
		imageData.CopyTo(_imageData);
	}

	public void Dispose()
	{
		_arrayPool.Return(_imageData);
	}

	private readonly ArrayPool<TPixel> _arrayPool;
	private readonly TPixel[] _imageData;
	private int ImageDataLength => ImageSize.X * ImageSize.Y;
}