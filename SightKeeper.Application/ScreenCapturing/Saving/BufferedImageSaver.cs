using System.Buffers;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.ScreenCapturing.Saving;

public sealed class BufferedImageSaver<TPixel> : ImageSaver<TPixel>, LimitedSaver, PendingImagesCountReporter, IDisposable
{
	private Vector2<ushort> MaximumImageSize
	{
		get;
		set
		{
			if (field == value)
				return;
			Guard.IsGreaterThan<ushort>(value.X, 0);
			Guard.IsGreaterThan<ushort>(value.Y, 0);
			field = value;
			UpdateArrayPools();
		}
	}

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

	public BufferedImageSaver(ImageDataAccess imageDataAccess, PixelConverter<TPixel, Rgba32> pixelConverter)
	{
		UpdateArrayPools();
		_imageDataAccess = imageDataAccess;
		_pixelConverter = pixelConverter;
		MaximumImageSize = new Vector2<ushort>(320, 320);
	}

	public void Dispose()
	{
		_pendingImagesCount.Dispose();
	}

	public void SaveImage(ImageSet set, ReadOnlySpan2D<TPixel> imageData)
	{
		Guard.IsFalse(IsLimitExceeded);
		var data = new ImageData<TPixel>(set, imageData, _rawPixelsArrayPool);
		_pendingImages.Enqueue(data);
		if (_processingTask.IsCompleted)
			_processingTask = Task.Run(ProcessImages);
		OnImagesCountChanged();
	}

	private readonly ConcurrentQueue<ImageData<TPixel>> _pendingImages = new();
	private readonly ImageDataAccess _imageDataAccess;
	private readonly PixelConverter<TPixel, Rgba32> _pixelConverter;
	private readonly BehaviorSubject<ushort> _pendingImagesCount = new(0);
	private ArrayPool<TPixel> _rawPixelsArrayPool;
	private ArrayPool<Rgba32> _convertedPixelsArrayPool;
	private Task _processingTask = Task.CompletedTask;
	private TaskCompletionSource? _limit;

	private ArrayPool<T> CreateArrayPool<T>()
	{
		return ArrayPool<T>.Create(MaximumImageSize.X * MaximumImageSize.Y, 50);
	}

	[MemberNotNull(nameof(_rawPixelsArrayPool), nameof(_convertedPixelsArrayPool))]
	private void UpdateArrayPools()
	{
		_rawPixelsArrayPool = CreateArrayPool<TPixel>();
		_convertedPixelsArrayPool = CreateArrayPool<Rgba32>();
	}

	private void ProcessImages()
	{
		while (_pendingImages.TryDequeue(out var data))
		{
			OnImagesCountChanged();
			var buffer = _convertedPixelsArrayPool.Rent(data.ImageSize.X * data.ImageSize.Y);
			try
			{
				var buffer2D = buffer.AsSpan().AsSpan2D(data.ImageSize.Y, data.ImageSize.X);
				_pixelConverter.Convert(data.Data2D, buffer2D);
				_imageDataAccess.CreateImage(data.Set, buffer2D, data.CreationTimestamp);
			}
			finally
			{
				_convertedPixelsArrayPool.Return(buffer);
				data.Dispose();
			}
		}
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