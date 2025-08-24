using System.Threading.Channels;
using CommunityToolkit.Diagnostics;
using Serilog;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application;

public sealed class ThreadedImageLoader<TPixel>(ImageLoader<TPixel> inner) : ImageLoader<TPixel>
{

	public Task<bool> LoadImageAsync(Image image, Memory<TPixel> target, CancellationToken cancellationToken)
	{
		var completionSource = new TaskCompletionSource<bool>();
		cancellationToken.Register(static state =>
		{
			var completionSource = (TaskCompletionSource<bool>)state!;
			if (completionSource.TrySetResult(false))
				Logger.Verbose("Image loading was canceled by cancellationToken registered callback");
		}, completionSource);
		var request = new LoadImageRequest(image, target, completionSource, cancellationToken);
		bool isWritten = _channel.Writer.TryWrite(request);
		Guard.IsTrue(isWritten);
		StartProcessingIfNecessary();
		return completionSource.Task;
	}

	private static readonly ILogger Logger = Log.Logger.ForContext<ThreadedImageLoader<TPixel>>();
	private readonly Channel<LoadImageRequest> _channel = Channel.CreateUnbounded<LoadImageRequest>();
	private readonly Lock _processingTaskLock = new();
	private Task _processingTask = Task.CompletedTask;

	private sealed record LoadImageRequest(
		Image Image,
		Memory<TPixel> Target,
		TaskCompletionSource<bool> CompletionSource,
		CancellationToken CancellationToken);

	private void StartProcessingIfNecessary()
	{
		if (!_processingTask.IsCompleted)
			return;
		lock (_processingTaskLock)
		{
			if (!_processingTask.IsCompleted)
				return;
			_processingTask = Task.Run(LoadImagesAsync);
		}
	}

	private async Task LoadImagesAsync()
	{
		while (_channel.Reader.TryRead(out var request))
		{
			bool isLoaded;
			try
			{
				isLoaded = await inner.LoadImageAsync(request.Image, request.Target, request.CancellationToken);
			}
			catch (Exception exception)
			{
				bool isSet = request.CompletionSource.TrySetException(exception);
				if (!isSet)
					Logger.Warning(exception, "An exception was thrown while loading an image {image} but could not be assigned to the TaskCompletionSource", request.Image);
				return;
			}
			if (request.CompletionSource.TrySetResult(isLoaded))
				Logger.Verbose("Threaded image {image} loading result set: {isLoaded}", request.Image, isLoaded);
		}
	}
}