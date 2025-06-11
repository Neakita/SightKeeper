using System.Collections.Concurrent;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ScreenCapturing.Saving;

public sealed class BufferedImageSaverMiddleware<TPixel> : ImageSaver<TPixel>, LimitedSaver, PendingImagesCountReporter, IDisposable
{
	public BehaviorObservable<ushort> PendingImagesCount => _pendingImagesCount;
	public required ImageSaver<TPixel> NextMiddleware { private get; init; }
	
	public ushort MaximumAllowedPendingImages
	{
		get;
		set
		{
			Guard.IsGreaterThan<ushort>(value, 0);
			field = value;
		}
	} = 10;

	public bool IsLimitReached => _pendingImages.Count == MaximumAllowedPendingImages;

	public Task Processing { get; private set; } = Task.CompletedTask;

	public void Dispose()
	{
		_pendingImagesCount.Dispose();
	}

	public void SaveImage(DomainImageSet set, ReadOnlySpan2D<TPixel> imageData, DateTimeOffset creationTimestamp)
	{
		Guard.IsFalse(IsLimitReached);
		var data = new ImageData<TPixel>(set, imageData, creationTimestamp);
		_pendingImages.Enqueue(data);
		OnImagesCountChanged();
		if (Processing.IsCompleted)
			Processing = Task.Run(ProcessImages);
	}

	private readonly ConcurrentQueue<ImageData<TPixel>> _pendingImages = new();
	private readonly BehaviorSubject<ushort> _pendingImagesCount = new(0);

	private void ProcessImages()
	{
		while (_pendingImages.TryDequeue(out var data))
		{
			OnImagesCountChanged();
			try
			{
				NextMiddleware.SaveImage(data.Set, data.Data2D, data.CreationTimestamp);
			}
			finally
			{
				data.Dispose();
			}
		}
	}

	private void OnImagesCountChanged()
	{
		var count = (ushort)_pendingImages.Count;
		_pendingImagesCount.OnNext(count);
	}
}