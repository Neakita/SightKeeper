using System;
using System.Buffers;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.HighPerformance;
using SightKeeper.Application.Screenshotting;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SixLabors.ImageSharp.PixelFormats;
using Yolo.InputProcessing;

namespace SightKeeper.Avalonia.Annotation.Screenshots;

internal sealed class ScreenshotImageLoader
{
	public ScreenshotImageLoader(WriteableBitmapPool bitmapPool, ScreenshotsDataAccess screenshotsDataAccess)
	{
		_bitmapPool = bitmapPool;
		_screenshotsDataAccess = screenshotsDataAccess;
	}

	public WriteableBitmap LoadImage(Screenshot screenshot)
	{
		PixelSize size = new(screenshot.ImageSize.X, screenshot.ImageSize.Y);
		WriteableBitmap bitmap = _bitmapPool.Rent(size, PixelFormat.Rgb32);
		using WriteableBitmapMemoryManager<Rgba32> bitmapMemoryManager = new(bitmap);
		ReadImageData(screenshot, bitmapMemoryManager.GetSpan());
		return bitmap;
	}

	public WriteableBitmap LoadImage(Screenshot screenshot, int maximumLargestDimension)
	{
		PixelSize size = ComputeThumbnailSize(screenshot, maximumLargestDimension);
		WriteableBitmap bitmap = _bitmapPool.Rent(size, PixelFormat.Rgb32);
		using WriteableBitmapMemoryManager<Rgba32> bitmapMemoryManager = new(bitmap);
		using var intermediateBuffer = MemoryPool<Rgba32>.Shared.Rent(screenshot.ImageSize.X * screenshot.ImageSize.Y);
		ReadImageData(screenshot, intermediateBuffer.Memory.Span);
		NearestNeighbourImageResizer.Resize(
			intermediateBuffer.Memory.Span.AsSpan2D(screenshot.ImageSize.Y, screenshot.ImageSize.X),
			bitmapMemoryManager.GetSpan().AsSpan2D(size.Height, size.Width));
		return bitmap;
	}

	public async Task<WriteableBitmap?> LoadImageAsync(Screenshot screenshot, CancellationToken cancellationToken)
	{
		PixelSize size = new(screenshot.ImageSize.X, screenshot.ImageSize.Y);
		WriteableBitmap bitmap = _bitmapPool.Rent(size, PixelFormat.Rgb32);
		using WriteableBitmapMemoryManager<Rgba32> bitmapMemoryManager = new(bitmap);
		bool read = await ReadImageDataAsync(screenshot, bitmapMemoryManager.Memory, cancellationToken);
		if (read)
			return bitmap;
		ReturnBitmapToPool(bitmap);
		return null;
	}

	public async Task<WriteableBitmap?> LoadImageAsync(Screenshot screenshot, int maximumLargestDimension, CancellationToken cancellationToken)
	{
		PixelSize size = ComputeThumbnailSize(screenshot, maximumLargestDimension);
		WriteableBitmap bitmap = _bitmapPool.Rent(size, PixelFormat.Rgb32);
		using WriteableBitmapMemoryManager<Rgba32> bitmapMemoryManager = new(bitmap);
		using var intermediateBuffer = MemoryPool<Rgba32>.Shared.Rent(screenshot.ImageSize.X * screenshot.ImageSize.Y);
		bool read = await ReadImageDataAsync(screenshot, intermediateBuffer.Memory, cancellationToken);
		if (!read)
		{
			ReturnBitmapToPool(bitmap);
			return null;
		}
		NearestNeighbourImageResizer.Resize(
			intermediateBuffer.Memory.Span.AsSpan2D(screenshot.ImageSize.Y, screenshot.ImageSize.X),
			bitmapMemoryManager.GetSpan().AsSpan2D(size.Height, size.Width));
		return bitmap;
	}

	public void ReturnBitmapToPool(WriteableBitmap bitmap)
	{
		_bitmapPool.Return(bitmap);
	}

	private readonly WriteableBitmapPool _bitmapPool;
	private readonly ScreenshotsDataAccess _screenshotsDataAccess;

	private void ReadImageData(Screenshot screenshot, Span<Rgba32> target)
	{
		Span<byte> targetAsBytes = MemoryMarshal.AsBytes(target);
		using var stream = _screenshotsDataAccess.LoadImage(screenshot);
		int totalBytesRead = 0;
		int lastBytesRead;
		do
		{
			lastBytesRead = stream.Read(targetAsBytes[totalBytesRead..]);
			totalBytesRead += lastBytesRead;
		} while (lastBytesRead > 0);
	}

	private async Task<bool> ReadImageDataAsync(Screenshot screenshot, Memory<Rgba32> target, CancellationToken cancellationToken)
	{
		Memory<byte> targetAsBytes = target.Cast<Rgba32, byte>();
		await using var stream = _screenshotsDataAccess.LoadImage(screenshot);
		int totalBytesRead = 0;
		int lastBytesRead;
		do
		{
			if (cancellationToken.IsCancellationRequested)
				return false;
			lastBytesRead = await stream.ReadAsync(targetAsBytes[totalBytesRead..], CancellationToken.None);
			totalBytesRead += lastBytesRead;
		} while (lastBytesRead > 0);
		return true;
	}

	private static PixelSize ComputeThumbnailSize(Screenshot screenshot, int maximumLargestDimension)
	{
		var sourceLargestDimension = Math.Max(screenshot.ImageSize.X, screenshot.ImageSize.Y);
		if (sourceLargestDimension < maximumLargestDimension)
			return new PixelSize(screenshot.ImageSize.X, screenshot.ImageSize.Y);
		Vector2<int> size = screenshot.ImageSize.ToInt32() * maximumLargestDimension / sourceLargestDimension;
		return new PixelSize(size.X, size.Y);
	}
}