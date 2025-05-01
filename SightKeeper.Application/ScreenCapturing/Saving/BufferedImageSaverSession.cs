using System.Buffers;
using System.Collections.Concurrent;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.ScreenCapturing.Saving;

public sealed class BufferedImageSaverSession<TPixel> : ImageSaverSession<TPixel>, LimitedSession
{
	public BehaviorObservable<ushort> PendingImagesCount => _pendingImagesCount;

	public ushort MaximumAllowedPendingImages
	{
		get;
		set
		{
			Guard.IsGreaterThan<ushort>(value, 0);
			field = value;
		}
	} = 10;

	public bool IsLimitExceeded => _limit != null;

	public Task Limit => _limit?.Task ?? Task.CompletedTask;

	public BufferedImageSaverSession(
		ImageSet imageSet,
		ImageDataAccess imageDataAccess,
		ArrayPool<TPixel> rawPixelsArrayPool,
		ArrayPool<Rgba32> convertedPixelsArrayPool,
		PixelConverter<TPixel, Rgba32> pixelConverter,
		ImagesCleaner imagesCleaner)
		: base(imageSet, imageDataAccess)
	{
		_pixelConverter = pixelConverter;
		_imagesCleaner = imagesCleaner;
		RawPixelsArrayPool = rawPixelsArrayPool;
		ConvertedPixelsArrayPool = convertedPixelsArrayPool;
	}

	public override void CreateImage(ReadOnlySpan2D<TPixel> imageData)
	{
		Guard.IsFalse(IsLimitExceeded);
		var data = new ImageData<TPixel>(imageData, RawPixelsArrayPool);
		_pendingImages.Enqueue(data);
		if (_processingTask.IsCompleted)
			_processingTask = Task.Run(ProcessImages);
		OnImagesCountChanged();
	}

	public override void Dispose()
	{
		_processingTask.Wait();
		Guard.IsEmpty(_pendingImages);
		_processingTask.Dispose();
		_pendingImagesCount.OnCompleted();
		_pendingImagesCount.Dispose();
	}

	internal ArrayPool<TPixel> RawPixelsArrayPool { get; set; }
	internal ArrayPool<Rgba32> ConvertedPixelsArrayPool { get; set; }

	private readonly ConcurrentQueue<ImageData<TPixel>> _pendingImages = new();
	private readonly BehaviorSubject<ushort> _pendingImagesCount = new(0);
	private readonly PixelConverter<TPixel, Rgba32> _pixelConverter;
	private readonly ImagesCleaner _imagesCleaner;
	private Task _processingTask = Task.CompletedTask;
	private TaskCompletionSource? _limit;

	private void ProcessImages()
	{
		while (_pendingImages.TryDequeue(out var data))
		{
			OnImagesCountChanged();
			var buffer = ConvertedPixelsArrayPool.Rent(data.ImageSize.X * data.ImageSize.Y);
			try
			{
				var buffer2D = buffer.AsSpan().AsSpan2D(data.ImageSize.Y, data.ImageSize.X);
				_pixelConverter.Convert(data.Data2D, buffer2D);
				ImageDataAccess.CreateImage(Set, buffer2D, data.CreationTimestamp);
			}
			finally
			{
				ConvertedPixelsArrayPool.Return(buffer);
				data.Dispose();
			}
		}
		_imagesCleaner.RemoveExceedUnusedImages(Set);
	}

	private void OnImagesCountChanged()
	{
		var count = (ushort)_pendingImages.Count;
		if (count > MaximumAllowedPendingImages)
		{
			Guard.IsNull(_limit);
			_limit = new TaskCompletionSource();
		}
		else if (count == 0 && _limit != null)
		{
			_limit.SetResult();
			_limit = null;
		}
		_pendingImagesCount.OnNext(count);
	}
}