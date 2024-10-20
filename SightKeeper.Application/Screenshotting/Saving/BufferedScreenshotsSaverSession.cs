using System.Buffers;
using System.Collections.Concurrent;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Screenshotting.Saving;

public sealed class BufferedScreenshotsSaverSession<TPixel> : ScreenshotsSaverSession<TPixel>
	where TPixel : unmanaged, IPixel<TPixel>
{
	public BehaviorObservable<ushort> PendingScreenshotsCount => _pendingScreenshotsCount;

	public BufferedScreenshotsSaverSession(
		ScreenshotsLibrary screenshotsLibrary,
		ScreenshotsDataAccess screenshotsDataAccess,
		ArrayPool<TPixel> arrayPool)
		: base(screenshotsLibrary, screenshotsDataAccess)
	{
		ArrayPool = arrayPool;
	}

	public override void CreateScreenshot(ReadOnlySpan2D<TPixel> imageData, DateTimeOffset creationDate)
	{
		ScreenshotData<TPixel> data = new(creationDate, imageData, ArrayPool);
		_pendingScreenshots.Enqueue(data);
		if (_processingTask.IsCompleted)
			_processingTask = Task.Run(ProcessScreenshots);
		UpdatePendingScreenshotsCount();
	}

	public override void Dispose()
	{
		_processingTask.Wait();
		Guard.IsEmpty(_pendingScreenshots);
		_processingTask.Dispose();
		_pendingScreenshotsCount.OnCompleted();
		_pendingScreenshotsCount.Dispose();
	}

	internal ArrayPool<TPixel> ArrayPool { get; set; }

	private readonly ConcurrentQueue<ScreenshotData<TPixel>> _pendingScreenshots = new();
	private readonly BehaviorSubject<ushort> _pendingScreenshotsCount = new(0);
	private Task _processingTask = Task.CompletedTask;

	private void ProcessScreenshots()
	{
		while (_pendingScreenshots.TryDequeue(out var data))
		{
			using var image = WrapSpanAsImage(data.ImageData, data.ImageSize);
			ScreenshotsDataAccess.CreateScreenshot(Library, image, data.CreationDate);
			data.Dispose();
			UpdatePendingScreenshotsCount();
		}
	}

	private static unsafe Image<TPixel> WrapSpanAsImage(ReadOnlySpan<TPixel> span, Vector2<ushort> imageSize)
	{
		fixed (TPixel* spanPointer = span)
		{
			var bufferSizeInBytes = sizeof(TPixel) * span.Length;
			return Image.WrapMemory<TPixel>(spanPointer, bufferSizeInBytes, imageSize.X, imageSize.Y);
		}
	}

	private void UpdatePendingScreenshotsCount()
	{
		_pendingScreenshotsCount.OnNext((ushort)_pendingScreenshots.Count);
	}
}