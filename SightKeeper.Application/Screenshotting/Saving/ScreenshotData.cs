using System.Buffers;
using CommunityToolkit.HighPerformance;
using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Screenshotting.Saving;

internal sealed class ScreenshotData<TPixel> : IDisposable
	where TPixel : unmanaged
{
	private readonly ArrayPool<TPixel> _arrayPool;
	public DateTimeOffset CreationDate { get; }
	public Vector2<ushort> ImageSize { get; }
	public ReadOnlySpan<TPixel> ImageData => _imageData.AsSpan()[.._imageDataLength];

	public ScreenshotData(
		DateTimeOffset creationDate,
		ReadOnlySpan2D<TPixel> imageData,
		ArrayPool<TPixel> arrayPool)
	{
		_arrayPool = arrayPool;
		CreationDate = creationDate;
		ImageSize = new Vector2<ushort>((ushort)imageData.Width, (ushort)imageData.Height);
		_imageDataLength = imageData.Width * imageData.Height;
		_imageData = _arrayPool.Rent(_imageDataLength);
		imageData.CopyTo(_imageData);
	}

	private readonly int _imageDataLength;
	private readonly TPixel[] _imageData;

	public void Dispose()
	{
		_arrayPool.Return(_imageData);
	}
}