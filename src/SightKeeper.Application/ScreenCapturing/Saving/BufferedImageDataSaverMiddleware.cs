using System.Buffers;
using System.Collections.Concurrent;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using Serilog;
using Serilog.Events;
using SerilogTimings.Extensions;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ScreenCapturing.Saving;

public sealed class BufferedImageDataSaverMiddleware<TPixel> : ImageDataSaver<TPixel>, LimitedSaver, PendingImagesCountReporter, IDisposable
{
	public required ImageDataSaver<TPixel> Next { get; init; }
	public required ArrayPool<TPixel> ArrayPool { get; init; }

	public ushort MaximumAllowedPendingImages
	{
		get;
		set
		{
			Guard.IsGreaterThan<ushort>(value, 0);
			field = value;
		}
	} = 10;

	public BehaviorObservable<ushort> PendingImagesCount => _pendingImagesCount;

	public bool IsLimitReached => _pendingImages.Count == MaximumAllowedPendingImages;

	public Task Processing { get; private set; } = Task.CompletedTask;

	public void SaveData(ManagedImage image, ReadOnlySpan2D<TPixel> data)
	{
		using var operation = Logger.BeginOperation("Creating image {image} data saving request", image);
		Guard.IsFalse(IsLimitReached);
		var pendingData = new PendingImageData<TPixel>(image, ArrayPool, data);
		_pendingImages.Enqueue(pendingData);
		OnPendingImagesCountChanged();
		if (Processing.IsCompleted)
			Processing = Task.Run(ProcessImages);
		operation.Complete(LogEventLevel.Verbose);
	}

	public void Dispose()
	{
		_pendingImagesCount.Dispose();
	}

	private static readonly ILogger Logger = Log.ForContext<BufferedImageDataSaverMiddleware<TPixel>>();
	private readonly ConcurrentQueue<PendingImageData<TPixel>> _pendingImages = new();
	private readonly BehaviorSubject<ushort> _pendingImagesCount = new(0);

	private void ProcessImages()
	{
		while (_pendingImages.TryDequeue(out var pendingData))
		{
			try
			{
				OnPendingImagesCountChanged();
				Next.SaveData(pendingData.Image, pendingData.Data);
			}
			finally
			{
				pendingData.Dispose();
			}
		}
	}

	private void OnPendingImagesCountChanged()
	{
		var count = (ushort)_pendingImages.Count;
		_pendingImagesCount.OnNext(count);
	}
}