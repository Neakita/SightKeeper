using System.Buffers;
using System.Collections.Concurrent;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using SightKeeper.Domain.Screenshots;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Screenshotting.Saving;

public sealed class BufferedScreenshotsSaverSession<TPixel> : ScreenshotsSaverSession<TPixel>, LimitedSession
{
	private readonly PixelConverter<TPixel, Rgba32> _pixelConverter;
	public BehaviorObservable<ushort> PendingScreenshotsCount => _pendingScreenshotsCount;

	public ushort MaximumAllowedPendingScreenshots
	{
		get => _maximumAllowedPendingScreenshots;
		set
		{
			Guard.IsGreaterThan<ushort>(value, 0);
			_maximumAllowedPendingScreenshots = value;
		}
	}

	public bool IsLimitExceeded => _limit != null;

	public Task Limit => _limit?.Task ?? Task.CompletedTask;

	public BufferedScreenshotsSaverSession(
		ScreenshotsLibrary screenshotsLibrary,
		ScreenshotsDataAccess screenshotsDataAccess,
		ArrayPool<TPixel> rawPixelsArrayPool,
		ArrayPool<Rgba32> convertedPixelsArrayPool,
		PixelConverter<TPixel, Rgba32> pixelConverter)
		: base(screenshotsLibrary, screenshotsDataAccess)
	{
		_pixelConverter = pixelConverter;
		RawPixelsArrayPool = rawPixelsArrayPool;
		ConvertedPixelsArrayPool = convertedPixelsArrayPool;
	}

	public override void CreateScreenshot(ReadOnlySpan2D<TPixel> imageData)
	{
		Guard.IsFalse(IsLimitExceeded);
		var data = new ScreenshotData<TPixel>(imageData, RawPixelsArrayPool);
		_pendingScreenshots.Enqueue(data);
		if (_processingTask.IsCompleted)
			_processingTask = Task.Run(ProcessScreenshots);
		OnScreenshotsCountChanged();
	}

	public override void Dispose()
	{
		_processingTask.Wait();
		Guard.IsEmpty(_pendingScreenshots);
		_processingTask.Dispose();
		_pendingScreenshotsCount.OnCompleted();
		_pendingScreenshotsCount.Dispose();
	}

	internal ArrayPool<TPixel> RawPixelsArrayPool { get; set; }
	internal ArrayPool<Rgba32> ConvertedPixelsArrayPool { get; set; }

	private readonly ConcurrentQueue<ScreenshotData<TPixel>> _pendingScreenshots = new();
	private readonly BehaviorSubject<ushort> _pendingScreenshotsCount = new(0);
	private Task _processingTask = Task.CompletedTask;
	private ushort _maximumAllowedPendingScreenshots = 10;
	private TaskCompletionSource? _limit;

	private void ProcessScreenshots()
	{
		while (_pendingScreenshots.TryDequeue(out var data))
		{
			OnScreenshotsCountChanged();
			var buffer = ConvertedPixelsArrayPool.Rent(data.ImageSize.X * data.ImageSize.Y);
			try
			{
				var buffer2D = buffer.AsSpan().AsSpan2D(data.ImageSize.Y, data.ImageSize.X);
				_pixelConverter.Convert(data.ImageData2D, buffer2D);
				ScreenshotsDataAccess.CreateScreenshot(Library, buffer2D, data.CreationDate);
			}
			finally
			{
				ConvertedPixelsArrayPool.Return(buffer);
				data.Dispose();
			}
		}
	}

	private void OnScreenshotsCountChanged()
	{
		var count = (ushort)_pendingScreenshots.Count;
		if (count > MaximumAllowedPendingScreenshots)
		{
			Guard.IsNull(_limit);
			_limit = new TaskCompletionSource();
		}
		else if (count == 0 && _limit != null)
		{
			_limit.SetResult();
			_limit = null;
		}
		_pendingScreenshotsCount.OnNext(count);
	}
}