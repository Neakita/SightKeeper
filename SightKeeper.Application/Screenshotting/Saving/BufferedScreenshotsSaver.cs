using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.HighPerformance;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Screenshotting.Saving;

public sealed class BufferedScreenshotsSaver<TPixel> : ScreenshotsSaver<TPixel>, PendingScreenshotsCountReporter, IDisposable
	where TPixel : unmanaged, IPixel<TPixel>
{
	public IObservable<ushort> PendingScreenshotsCount => _pendingScreenshotsCount.AsObservable();

	public BufferedScreenshotsSaver(ScreenshotsDataAccess screenshotsDataAccess)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
	}

	public override void CreateScreenshot(ScreenshotsLibrary library, ReadOnlySpan2D<TPixel> imageData, DateTimeOffset creationDate)
	{
		ScreenshotData<TPixel> data = new(library, creationDate, imageData);
		_pendingScreenshots.Enqueue(data);
		UpdatePendingScreenshotsCount();
		if (_processingTask.IsCompleted)
			_processingTask = Task.Run(ProcessScreenshots);
	}

	public override void Dispose()
	{
		_pendingScreenshotsCount.Dispose();
	}

	private readonly ScreenshotsDataAccess _screenshotsDataAccess;
	private readonly ConcurrentQueue<ScreenshotData<TPixel>> _pendingScreenshots = new();
	private readonly BehaviorSubject<ushort> _pendingScreenshotsCount = new(0);
	private Task _processingTask = Task.CompletedTask;

	private void ProcessScreenshots()
	{
		while (_pendingScreenshots.TryDequeue(out var data))
		{
			using var image = WrapSpanAsImage(data.ImageData, data.ImageSize);
			_screenshotsDataAccess.CreateScreenshot(data.Library, image, data.CreationDate);
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