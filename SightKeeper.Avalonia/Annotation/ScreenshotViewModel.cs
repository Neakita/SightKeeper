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
using Yolo;

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
		using var buffer = bitmap.Lock();
		
		Span<Rgba32> span = new((void*)buffer.Address, _screenshot.ImageSize.X * _screenshot.ImageSize.Y);
		Span<byte> bytesSpan = MemoryMarshal.AsBytes(span);
		using var stream = _screenshotsDataAccess.LoadImage(_screenshot);
		var bytesRead = 0;
		while (stream.CanRead)
		{
			var read = stream.Read(bytesSpan[bytesRead..]);
			if (read == 0)
				break;
			bytesRead += read;
		}

		return bitmap;
	}

	private unsafe WriteableBitmap LoadImage(int maximumLargestDimension)
	{
		PixelSize size = ComputeThumbnailSize(maximumLargestDimension);
		WriteableBitmap bitmap = _bitmapPool.Rent(size, PixelFormat.Rgb32);
		using var buffer = bitmap.Lock();
		Span<Rgba32> span = new((void*)buffer.Address, size.Width * size.Height);
		var arrayBuffer = ArrayPool<Rgba32>.Shared.Rent(_screenshot.ImageSize.X * _screenshot.ImageSize.Y);
		Span<byte> bytesSpan = MemoryMarshal.AsBytes(arrayBuffer.AsSpan());
		using var stream = _screenshotsDataAccess.LoadImage(_screenshot);
		var bytesRead = 0;
		while (stream.CanRead)
		{
			var read = stream.Read(bytesSpan[bytesRead..]);
			if (read == 0)
				break;
			bytesRead += read;
		}
		NearestNeighbourImageResizer.Resize(arrayBuffer.AsSpan().AsSpan2D(_screenshot.ImageSize.Y, _screenshot.ImageSize.X), span.AsSpan2D(size.Height, size.Width));
		ArrayPool<Rgba32>.Shared.Return(arrayBuffer);
		return bitmap;
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