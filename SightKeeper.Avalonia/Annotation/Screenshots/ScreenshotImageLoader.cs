using System;
using System.Buffers;
using System.Runtime.InteropServices;
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

	public unsafe WriteableBitmap LoadImage(Screenshot screenshot)
	{
		PixelSize size = new(screenshot.ImageSize.X, screenshot.ImageSize.Y);
		WriteableBitmap bitmap = _bitmapPool.Rent(size, PixelFormat.Rgb32);
		using var bitmapBuffer = bitmap.Lock();
		Span<Rgba32> bitmapSpan = new((void*)bitmapBuffer.Address, screenshot.ImageSize.X * screenshot.ImageSize.Y);
		ReadImageData(screenshot, bitmapSpan);
		return bitmap;
	}

	public unsafe WriteableBitmap LoadImage(Screenshot screenshot, int maximumLargestDimension)
	{
		PixelSize size = ComputeThumbnailSize(screenshot, maximumLargestDimension);
		WriteableBitmap bitmap = _bitmapPool.Rent(size, PixelFormat.Rgb32);
		using var bitmapBuffer = bitmap.Lock();
		Span<Rgba32> bitmapSpan = new((void*)bitmapBuffer.Address, size.Width * size.Height);
		var intermediateBuffer = ArrayPool<Rgba32>.Shared.Rent(screenshot.ImageSize.X * screenshot.ImageSize.Y);
		ReadImageData(screenshot, intermediateBuffer);
		NearestNeighbourImageResizer.Resize(intermediateBuffer.AsSpan().AsSpan2D(screenshot.ImageSize.Y, screenshot.ImageSize.X), bitmapSpan.AsSpan2D(size.Height, size.Width));
		ArrayPool<Rgba32>.Shared.Return(intermediateBuffer);
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
		var bytesRead = 0;
		while (stream.CanRead)
		{
			var read = stream.Read(targetAsBytes[bytesRead..]);
			if (read == 0)
				break;
			bytesRead += read;
		}
	}

	private PixelSize ComputeThumbnailSize(Screenshot screenshot, int maximumLargestDimension)
	{
		var sourceLargestDimension = Math.Max(screenshot.ImageSize.X, screenshot.ImageSize.Y);
		if (sourceLargestDimension < maximumLargestDimension)
			return new PixelSize(screenshot.ImageSize.X, screenshot.ImageSize.Y);
		Vector2<int> size = screenshot.ImageSize.ToInt32() * maximumLargestDimension / sourceLargestDimension;
		return new PixelSize(size.X, size.Y);
	}
}