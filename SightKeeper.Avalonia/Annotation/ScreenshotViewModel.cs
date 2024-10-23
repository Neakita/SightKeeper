using System;
using System.Buffers;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.HighPerformance;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application.Screenshotting;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SixLabors.ImageSharp.PixelFormats;
using Yolo.InputProcessing;

namespace SightKeeper.Avalonia.Annotation;

internal sealed partial class ScreenshotViewModel : ViewModel
{
	public WriteableBitmap Image => LoadImage();
	public WriteableBitmap Thumbnail => LoadImage(100);

	public ScreenshotViewModel(
		Screenshot screenshot,
		ScreenshotsDataAccess screenshotsDataAccess,
		WriteableBitmapPool bitmapPool)
	{
		_screenshot = screenshot;
		_screenshotsDataAccess = screenshotsDataAccess;
		_bitmapPool = bitmapPool;
	}

	private readonly Screenshot _screenshot;
	private readonly ScreenshotsDataAccess _screenshotsDataAccess;
	private readonly WriteableBitmapPool _bitmapPool;

	private unsafe WriteableBitmap LoadImage()
	{
		PixelSize size = new(_screenshot.ImageSize.X, _screenshot.ImageSize.Y);
		WriteableBitmap bitmap = _bitmapPool.Rent(size, PixelFormat.Rgb32);
		using var bitmapBuffer = bitmap.Lock();
		Span<Rgba32> bitmapSpan = new((void*)bitmapBuffer.Address, _screenshot.ImageSize.X * _screenshot.ImageSize.Y);
		ReadImageData(bitmapSpan);
		return bitmap;
	}

	internal unsafe WriteableBitmap LoadImage(int maximumLargestDimension)
	{
		PixelSize size = ComputeThumbnailSize(maximumLargestDimension);
		WriteableBitmap bitmap = _bitmapPool.Rent(size, PixelFormat.Rgb32);
		using var bitmapBuffer = bitmap.Lock();
		Span<Rgba32> bitmapSpan = new((void*)bitmapBuffer.Address, size.Width * size.Height);
		var intermediateBuffer = ArrayPool<Rgba32>.Shared.Rent(_screenshot.ImageSize.X * _screenshot.ImageSize.Y);
		ReadImageData(intermediateBuffer);
		NearestNeighbourImageResizer.Resize(intermediateBuffer.AsSpan().AsSpan2D(_screenshot.ImageSize.Y, _screenshot.ImageSize.X), bitmapSpan.AsSpan2D(size.Height, size.Width));
		ArrayPool<Rgba32>.Shared.Return(intermediateBuffer);
		return bitmap;
	}

	private void ReadImageData(Span<Rgba32> target)
	{
		Span<byte> targetAsBytes = MemoryMarshal.AsBytes(target);
		using var stream = _screenshotsDataAccess.LoadImage(_screenshot);
		var bytesRead = 0;
		while (stream.CanRead)
		{
			var read = stream.Read(targetAsBytes[bytesRead..]);
			if (read == 0)
				break;
			bytesRead += read;
		}
	}

	private PixelSize ComputeThumbnailSize(int maximumLargestDimension)
	{
		var sourceLargestDimension = Math.Max(_screenshot.ImageSize.X, _screenshot.ImageSize.Y);
		if (sourceLargestDimension < maximumLargestDimension)
			return new PixelSize(_screenshot.ImageSize.X, _screenshot.ImageSize.Y);
		Vector2<int> size = _screenshot.ImageSize.ToInt32() * maximumLargestDimension / sourceLargestDimension;
		return new PixelSize(size.X, size.Y);
	}

	[RelayCommand]
	private void ReturnBitmapToPool(WriteableBitmap bitmap)
	{
		_bitmapPool.Return(bitmap);
	}
}