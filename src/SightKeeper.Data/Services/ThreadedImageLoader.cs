using System.Threading.Channels;
using CommunityToolkit.Diagnostics;
using Serilog;
using Serilog.Events;
using SerilogTimings.Extensions;
using SightKeeper.Application.Misc;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Services;

internal sealed class ThreadedImageLoader<TPixel> : ImageLoader<TPixel>
{
	public ThreadedImageLoader(ImageLoader<TPixel> inner, ILogger logger)
	{
		_inner = inner;
		_logger = logger;
		Task.Run(ProcessRequestsAsync);
	}

	public Task<bool> LoadImageAsync(ImageData imageData, Memory<TPixel> target, CancellationToken cancellationToken)
	{
		var completionSource = new TaskCompletionSource<bool>();
		cancellationToken.Register(() =>
		{
			if (completionSource.TrySetResult(false))
				_logger.Verbose("Image loading was canceled by cancellationToken registered callback");
		});
		var request = new LoadImageRequest(imageData, target, completionSource, cancellationToken);
		bool isWritten = _requestsChannel.Writer.TryWrite(request);
		Guard.IsTrue(isWritten);
		return completionSource.Task;
	}

	private readonly Channel<LoadImageRequest> _requestsChannel = Channel.CreateUnbounded<LoadImageRequest>();
	private readonly ImageLoader<TPixel> _inner;
	private readonly ILogger _logger;

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
		using var operation = _logger.OperationAt(LogEventLevel.Verbose).Begin("Image loading request processing");
		try
		{
			await ProcessRequestAsync(request);
			operation.Complete();
		}
		catch (TaskCanceledException exception)
		{
			var isResultSet = request.CompletionSource.TrySetResult(false);
			_logger.Verbose(exception, "Image {image} loading was cancelled via an exception. Is result set: {isResultSet}", request.Image, isResultSet);
			operation.SetException(exception);
		}
		catch (Exception exception)
		{
			var isExceptionSet = request.CompletionSource.TrySetException(exception);
			if (!isExceptionSet)
				_logger.Warning(exception, "An exception was thrown while processing image {image} load request, but couldn't be set to CompletionSource", request.Image);
			operation.SetException(exception);
		}
	}

	private async Task ProcessRequestAsync(LoadImageRequest request)
	{
		if (request.CancellationToken.IsCancellationRequested)
			return;
		var isLoaded = await _inner.LoadImageAsync(request.Image, request.Target, request.CancellationToken);
		if (request.CompletionSource.TrySetResult(isLoaded))
			_logger.Verbose("Threaded image {image} loading result set: {isLoaded}", request.Image, isLoaded);
	}
}