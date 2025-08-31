using System.Threading.Channels;
using CommunityToolkit.Diagnostics;
using Serilog;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application;

public sealed class ThreadedImageLoader<TPixel> : ImageLoader<TPixel>
{
	public bool LoadImage(ImageData imageData, Memory<TPixel> target, CancellationToken cancellationToken)
	{
		// there is nothing we can do
		return _inner.LoadImage(imageData, target, cancellationToken);
	}

	public Task<bool> LoadImageAsync(ImageData imageData, Memory<TPixel> target, CancellationToken cancellationToken)
	{
		var completionSource = new TaskCompletionSource<bool>();
		cancellationToken.Register(static state =>
		{
			var completionSource = (TaskCompletionSource<bool>)state!;
			if (completionSource.TrySetResult(false))
				Logger.Verbose("Image loading was canceled by cancellationToken registered callback");
		}, completionSource);
		var request = new LoadImageRequest(imageData, target, completionSource, cancellationToken);
		bool isWritten = _channel.Writer.TryWrite(request);
		Guard.IsTrue(isWritten);
		return completionSource.Task;
	}

	private static readonly ILogger Logger = Log.Logger.ForContext<ThreadedImageLoader<TPixel>>();
	private readonly Channel<LoadImageRequest> _channel = Channel.CreateUnbounded<LoadImageRequest>();
	private readonly ImageLoader<TPixel> _inner;

	public ThreadedImageLoader(ImageLoader<TPixel> inner)
	{
		_inner = inner;
		_ = new TaskFactory().StartNew(LoadImages, TaskCreationOptions.LongRunning);
	}

	private sealed record LoadImageRequest(
		ImageData Image,
		Memory<TPixel> Target,
		TaskCompletionSource<bool> CompletionSource,
		CancellationToken CancellationToken);

	private void LoadImages()
	{
		while (_channel.Reader.TryRead(out var request))
		{
			if (request.CancellationToken.IsCancellationRequested)
				continue;
			bool isLoaded;
			try
			{
				isLoaded = _inner.LoadImage(request.Image, request.Target, request.CancellationToken);
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