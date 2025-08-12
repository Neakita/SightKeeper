using System.Buffers;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ScreenCapturing.Saving;

internal sealed class PendingImageData<TPixel> : IDisposable
{
	public Image Image { get; }

	public ReadOnlySpan2D<TPixel> Data => _rentedArray.AsSpan2D(Image.Size.Y, Image.Size.X);

	public PendingImageData(Image image, ArrayPool<TPixel> arrayPool, ReadOnlySpan2D<TPixel> data)
	{
		Guard.IsEqualTo(image.Size.X, data.Width);
		Guard.IsEqualTo(image.Size.Y, data.Height);
		Image = image;
		_arrayPool = arrayPool;
		_rentedArray = arrayPool.Rent(ImageDataLength);
		data.CopyTo(_rentedArray);
	}

	public void Dispose()
	{
		_arrayPool.Return(_rentedArray);
	}

	private readonly ArrayPool<TPixel> _arrayPool;
	private readonly TPixel[] _rentedArray;
	private int ImageDataLength => Image.Size.X * Image.Size.Y;
}