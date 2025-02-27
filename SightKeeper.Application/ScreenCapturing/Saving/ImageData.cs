using System.Buffers;
using CommunityToolkit.HighPerformance;
using SightKeeper.Domain;

namespace SightKeeper.Application.ScreenCapturing.Saving;

internal sealed class ImageData<TPixel> : IDisposable
{
	public DateTimeOffset CreationTimestamp { get; } = DateTimeOffset.Now;
	public Vector2<ushort> ImageSize { get; }
	public ReadOnlySpan<TPixel> Data => _data.AsSpan()[..ImageDataLength];
	public ReadOnlySpan2D<TPixel> Data2D => Data.AsSpan2D(ImageSize.Y, ImageSize.X);

	public ImageData(
		ReadOnlySpan2D<TPixel> imageData,
		ArrayPool<TPixel> arrayPool)
	{
		_arrayPool = arrayPool;
		ImageSize = new Vector2<ushort>((ushort)imageData.Width, (ushort)imageData.Height);
		_data = arrayPool.Rent(ImageDataLength);
		imageData.CopyTo(_data);
	}

	public void Dispose()
	{
		_arrayPool.Return(_data);
	}

	private readonly ArrayPool<TPixel> _arrayPool;
	private readonly TPixel[] _data;
	private int ImageDataLength => ImageSize.X * ImageSize.Y;
}