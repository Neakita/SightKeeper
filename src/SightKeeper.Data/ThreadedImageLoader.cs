using System.Threading.Channels;
using CommunityToolkit.Diagnostics;
using Serilog;
using SightKeeper.Application;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data;

internal sealed class ThreadedImageLoader<TPixel> : ImageLoader<TPixel>
{
	public ThreadedImageLoader(ImageLoader<TPixel> inner)
	{
		_inner = inner;
		Task.Run(ProcessRequestsAsync);
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
		bool isWritten = _requestsChannel.Writer.TryWrite(request);
		Guard.IsTrue(isWritten);
		return completionSource.Task;
	}

	private static readonly ILogger Logger = Log.Logger.ForContext<ThreadedImageLoader<TPixel>>();
	private readonly Channel<LoadImageRequest> _requestsChannel = Channel.CreateUnbounded<LoadImageRequest>();
	private readonly ImageLoader<TPixel> _inner;

	private sealed record LoadImageRequest(
		ImageData Image,
		Memory<TPixel> Target,
		TaskCompletionSource<bool> CompletionSource,
		CancellationToken CancellationToken);

	private async Task ProcessRequestsAsync()
	{
		while (await _requestsChannel.Reader.WaitToReadAsync())
		while (_requestsChannel.Reader.TryRead(out var request))
			await TryProcessRequestAsync(request);
	}

	private async Task TryProcessRequestAsync(LoadImageRequest request)
	{
		try
		{
			await ProcessRequestAsync(request);
		}
		catch (Exception exception)
		{
			var isExceptionSet = request.CompletionSource.TrySetException(exception);
			if (!isExceptionSet)
				Log.Logger.Warning(exception, "An exception was thrown while processing image {image} load request, but couldn't be set to CompletionSource", request.Image);
		}
	}

	private async Task ProcessRequestAsync(LoadImageRequest request)
	{
		if (request.CancellationToken.IsCancellationRequested)
			return;
		var isLoaded = await _inner.LoadImageAsync(request.Image, request.Target, request.CancellationToken);
		if (request.CompletionSource.TrySetResult(isLoaded))
			Logger.Verbose("Threaded image {image} loading result set: {isLoaded}", request.Image, isLoaded);
	}
}