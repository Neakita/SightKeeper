using System;
using System.Buffers;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.HighPerformance;
using SightKeeper.Application.Screenshotting;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp.PixelFormats;
using Yolo.InputProcessing;

namespace SightKeeper.Avalonia.Annotation.Screenshots;

public sealed class ScreenshotImageLoader
{
	public ScreenshotImageLoader(WriteableBitmapPool bitmapPool, ScreenshotsDataAccess screenshotsDataAccess)
	{
		_bitmapPool = bitmapPool;
		_screenshotsDataAccess = screenshotsDataAccess;
	}

	public async Task<WriteableBitmap?> LoadImageAsync(
		Image image,
		int? maximumLargestDimension,
		CancellationToken cancellationToken)
	{
		PixelSize size = maximumLargestDimension == null
			? image.ImageSize.ToPixelSize()
			: ComputeSize(image, maximumLargestDimension.Value);
		WriteableBitmap bitmap = _bitmapPool.Rent(size, PixelFormat.Rgb32);
		bool isRead = await ReadImageDataToBitmapAsync(image, bitmap, cancellationToken);
		if (isRead)
			return bitmap;
		ReturnBitmapToPool(bitmap);
		return null;
	}

	public void ReturnBitmapToPool(WriteableBitmap bitmap)
	{
		_bitmapPool.Return(bitmap);
	}

	private readonly WriteableBitmapPool _bitmapPool;
	private readonly ScreenshotsDataAccess _screenshotsDataAccess;

	private async Task<bool> ReadImageDataToBitmapAsync(
		Image image,
		WriteableBitmap bitmap,
		CancellationToken cancellationToken)
	{
		using WriteableBitmapMemoryManager<Rgba32> bitmapMemoryManager = new(bitmap);
		var screenshotSize = image.ImageSize.ToPixelSize();
		if (screenshotSize == bitmap.PixelSize)
			return await ReadImageDataAsync(image, bitmapMemoryManager.Memory, cancellationToken);
		var pixelsCount = screenshotSize.Width * screenshotSize.Height;
		using var buffer = MemoryPool<Rgba32>.Shared.Rent(pixelsCount);
		bool isRead = await ReadImageDataAsync(image, buffer.Memory, cancellationToken);
		if (!isRead)
			return false;
		NearestNeighbourImageResizer.Resize(
			buffer.Memory.Span.AsSpan2D(screenshotSize.Height, screenshotSize.Width),
			bitmapMemoryManager.Memory.Span.AsSpan2D(bitmap.PixelSize.Height, bitmap.PixelSize.Width));
		return true;
	}

	private async Task<bool> ReadImageDataAsync(
		Image image,
		Memory<Rgba32> target,
		CancellationToken cancellationToken)
	{
		Memory<byte> targetAsBytes = target.Cast<Rgba32, byte>();
		await using var stream = _screenshotsDataAccess.LoadImage(image);
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

	private static PixelSize ComputeSize(Image image, int maximumLargestDimension)
	{
		var sourceLargestDimension = Math.Max(image.ImageSize.X, image.ImageSize.Y);
		if (sourceLargestDimension < maximumLargestDimension)
			return new PixelSize(image.ImageSize.X, image.ImageSize.Y);
		Vector2<int> size = image.ImageSize.ToInt32() * maximumLargestDimension / sourceLargestDimension;
		return new PixelSize(size.X, size.Y);
	}
}