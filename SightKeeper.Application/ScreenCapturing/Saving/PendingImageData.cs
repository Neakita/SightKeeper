using System.Buffers;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ScreenCapturing.Saving;

internal sealed class PendingImageData<TPixel> : IDisposable
{
	public Image Image { get; }

	public ReadOnlySpan2D<TPixel> Data
	{
		get
		{
			var memory = _memoryOwner.Memory;
			var memory2D = memory.AsMemory2D(Image.Size.Y, Image.Size.X);
			return memory2D.Span;
		}
	}

	public PendingImageData(Image image, ReadOnlySpan2D<TPixel> data)
	{
		Guard.IsEqualTo(image.Size.X, data.Width);
		Guard.IsEqualTo(image.Size.Y, data.Height);
		Image = image;
		_memoryOwner = MemoryPool<TPixel>.Shared.Rent(ImageDataLength);
		data.CopyTo(_memoryOwner.Memory.Span);
	}

	public void Dispose()
	{
		_memoryOwner.Dispose();
	}

	private readonly IMemoryOwner<TPixel> _memoryOwner;
	private int ImageDataLength => Image.Size.X * Image.Size.Y;
}