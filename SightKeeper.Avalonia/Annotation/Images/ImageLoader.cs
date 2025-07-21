using System;
using System.Buffers;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.HighPerformance;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp.PixelFormats;
using Yolo.InputProcessing;

namespace SightKeeper.Avalonia.Annotation.Images;

public sealed class ImageLoader
{
	public ImageLoader(WriteableBitmapPool bitmapPool)
	{
		_bitmapPool = bitmapPool;
	}

	public async Task<PooledWriteableBitmap?> LoadImageAsync(
		Image image,
		int? maximumLargestDimension,
		CancellationToken cancellationToken)
	{
		PixelSize size = maximumLargestDimension == null
			? image.Size.ToPixelSize()
			: ComputeSize(image.Size, maximumLargestDimension.Value);
		PooledWriteableBitmap bitmap = _bitmapPool.Rent(size, PixelFormat.Rgb32);
		bool isRead = await ReadImageDataToBitmapAsync(image, bitmap, cancellationToken);
		if (isRead)
			return bitmap;
		bitmap.ReturnToPool();
		return null;
	}

	private readonly WriteableBitmapPool _bitmapPool;

	private async Task<bool> ReadImageDataToBitmapAsync(
		Image image,
		WriteableBitmap bitmap,
		CancellationToken cancellationToken)
	{
		using WriteableBitmapMemoryManager<Rgba32> bitmapMemoryManager = new(bitmap);
		var imageSize = image.Size.ToPixelSize();
		if (imageSize == bitmap.PixelSize)
			return await ReadImageDataAsync(image, bitmapMemoryManager.Memory, cancellationToken);
		var pixelsCount = imageSize.Width * imageSize.Height;
		using var buffer = MemoryPool<Rgba32>.Shared.Rent(pixelsCount);
		bool isRead = await ReadImageDataAsync(image, buffer.Memory, cancellationToken);
		if (!isRead)
			return false;
		NearestNeighbourImageResizer.Resize(
			buffer.Memory.Span.AsSpan2D(imageSize.Height, imageSize.Width),
			bitmapMemoryManager.Memory.Span.AsSpan2D(bitmap.PixelSize.Height, bitmap.PixelSize.Width));
		return true;
	}

	private async Task<bool> ReadImageDataAsync(
		Image image,
		Memory<Rgba32> target,
		CancellationToken cancellationToken)
	{
		Memory<byte> targetAsBytes = target.Cast<Rgba32, byte>();
		await using var stream = image.OpenReadStream();
		if (stream == null)
			return false;
		int totalBytesRead = 0;
		int lastBytesRead;
		do
		{
			// ReadAsync method will throw exception on await if token is canceled and thus degrade performance,
			// so it's better to manually check the token and return boolean value instead of re-throwing or whatever.
			if (cancellationToken.IsCancellationRequested)
				return false;
			// CancellationToken.None passed as parameter to suppress analyzer.
			lastBytesRead = await stream.ReadAsync(targetAsBytes[totalBytesRead..], CancellationToken.None);
			totalBytesRead += lastBytesRead;
		} while (lastBytesRead > 0);
		return true;
	}

	public static PixelSize ComputeSize(Vector2<ushort> imageSize, int maximumLargestDimension)
	{
		var sourceLargestDimension = Math.Max(imageSize.X, imageSize.Y);
		if (sourceLargestDimension < maximumLargestDimension)
			return new PixelSize(imageSize.X, imageSize.Y);
		Vector2<int> size = imageSize.ToInt32() * maximumLargestDimension / sourceLargestDimension;
		return new PixelSize(size.X, size.Y);
	}
}