using System;
using System.Buffers;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.HighPerformance;
using SightKeeper.Application.Misc;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;
using TensorWeaver.InputProcessing;

namespace SightKeeper.Avalonia.Annotation.Images;

public sealed class WriteableBitmapImageLoader<TPixel>(
	WriteableBitmapPool bitmapPool,
	PixelFormat pixelFormat,
	ImageLoader<TPixel> imageLoader)
	: WriteableBitmapImageLoader
	where TPixel : unmanaged
{
	public async Task<PooledWriteableBitmap?> LoadImageAsync(
		ManagedImage image,
		int? maximumLargestDimension,
		CancellationToken cancellationToken)
	{
		var size = maximumLargestDimension == null
			? image.Size.ToPixelSize()
			: ComputeSize(image.Size, maximumLargestDimension.Value);
		var bitmap = bitmapPool.Rent(size, pixelFormat);
		bool isRead;
		try
		{
			isRead = await ReadImageDataToBitmapAsync(image, bitmap, cancellationToken);
		}
		catch
		{
			bitmap.ReturnToPool();
			throw;
		}
		if (isRead)
			return bitmap;
		bitmap.ReturnToPool();
		return null;
	}

	private async Task<bool> ReadImageDataToBitmapAsync(
		ManagedImage image,
		WriteableBitmap bitmap,
		CancellationToken cancellationToken)
	{
		using WriteableBitmapMemoryManager<TPixel> bitmapMemoryManager = new(bitmap);
		var imageSize = image.Size.ToPixelSize();
		if (imageSize == bitmap.PixelSize)
			return await imageLoader.LoadImageAsync(image, bitmapMemoryManager.Memory, cancellationToken);
		var pixelsCount = imageSize.Width * imageSize.Height;
		using var buffer = MemoryPool<TPixel>.Shared.Rent(pixelsCount);
		var isRead = await imageLoader.LoadImageAsync(image, buffer.Memory, cancellationToken);
		if (!isRead)
			return false;
		NearestNeighbourImageResizer.Resize(
			buffer.Memory.Span.AsSpan2D(imageSize.Height, imageSize.Width),
			bitmapMemoryManager.Memory.Span.AsSpan2D(bitmap.PixelSize.Height, bitmap.PixelSize.Width));
		return true;
	}

	private static PixelSize ComputeSize(Vector2<ushort> imageSize, int maximumLargestDimension)
	{
		var sourceLargestDimension = Math.Max(imageSize.X, imageSize.Y);
		if (sourceLargestDimension < maximumLargestDimension)
			return new PixelSize(imageSize.X, imageSize.Y);
		var size = imageSize.ToInt32() * maximumLargestDimension / sourceLargestDimension;
		return new PixelSize(size.X, size.Y);
	}
}