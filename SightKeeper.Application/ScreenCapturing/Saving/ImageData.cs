using System.Buffers;
using CommunityToolkit.HighPerformance;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ScreenCapturing.Saving;

internal sealed class ImageData<TPixel> : IDisposable
{
	public DomainImageSet Set { get; }
	public DateTimeOffset CreationTimestamp { get; }
	public Vector2<ushort> ImageSize { get; }
	public ReadOnlySpan<TPixel> Data => _memoryOwner.Memory.Span[..ImageDataLength];
	public ReadOnlySpan2D<TPixel> Data2D => Data.AsSpan2D(ImageSize.Y, ImageSize.X);

	public ImageData(
		DomainImageSet set,
		ReadOnlySpan2D<TPixel> imageData,
		DateTimeOffset creationTimestamp)
	{
		Set = set;
		ImageSize = new Vector2<ushort>((ushort)imageData.Width, (ushort)imageData.Height);
		_memoryOwner = MemoryPool<TPixel>.Shared.Rent(ImageSize.X * ImageSize.Y);
		imageData.CopyTo(_memoryOwner.Memory.Span);
		CreationTimestamp = creationTimestamp;
	}

	public void Dispose()
	{
		_memoryOwner.Dispose();
	}

	private readonly IMemoryOwner<TPixel> _memoryOwner;
	private int ImageDataLength => ImageSize.X * ImageSize.Y;
}