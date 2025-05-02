using System.Buffers;
using System.Collections.Concurrent;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.ScreenCapturing.Saving;

public sealed class BufferedImageSaver<TPixel> : ImageSaver<TPixel>, LimitedSaver, PendingImagesCountReporter, IDisposable
{
	public BehaviorObservable<ushort> PendingImagesCount => _pendingImagesCount;
	
	public ushort MaximumAllowedPendingImages
	{
		get;
		set
		{
			Guard.IsGreaterThan<ushort>(value, 0);
			field = value;
			_rawPixelsPool = null;
		}
	} = 10;

	public bool IsLimitReached => _pendingImages.Count == MaximumAllowedPendingImages;

	public Task Processing { get; private set; } = Task.CompletedTask;

	public BufferedImageSaver(ImageDataAccess imageDataAccess, PixelConverter<TPixel, Rgba32> pixelConverter)
	{
		_imageDataAccess = imageDataAccess;
		_pixelConverter = pixelConverter;
	}

	public void Dispose()
	{
		_pendingImagesCount.Dispose();
	}

	public void SaveImage(ImageSet set, ReadOnlySpan2D<TPixel> imageData)
	{
		GrowMaximumImageDataLengthIfNecessary(new Vector2<ushort>((ushort)imageData.Width, (ushort)imageData.Height));
		Guard.IsFalse(IsLimitReached);
		var data = new ImageData<TPixel>(set, imageData, RawPixelsPool);
		_pendingImages.Enqueue(data);
		OnImagesCountChanged();
		if (Processing.IsCompleted)
			Processing = Task.Run(ProcessImages);
	}

	private int MaximumImageDataLength
	{
		get;
		set
		{
			if (field == value)
				return;
			Guard.IsGreaterThanOrEqualTo(value, field);
			field = value;
			_rawPixelsPool = null;
			_convertedPixelsBuffer = null;
		}
	}

	private ArrayPool<TPixel> RawPixelsPool => _rawPixelsPool ??=
		ArrayPool<TPixel>.Create(MaximumImageDataLength, MaximumAllowedPendingImages);

	private Rgba32[] ConvertedPixelsBuffer => _convertedPixelsBuffer ?? new Rgba32[MaximumImageDataLength];

	private readonly ConcurrentQueue<ImageData<TPixel>> _pendingImages = new();
	private readonly ImageDataAccess _imageDataAccess;
	private readonly PixelConverter<TPixel, Rgba32> _pixelConverter;
	private readonly BehaviorSubject<ushort> _pendingImagesCount = new(0);
	private ArrayPool<TPixel>? _rawPixelsPool;
	private Rgba32[]? _convertedPixelsBuffer;

	private void GrowMaximumImageDataLengthIfNecessary(Vector2<ushort> imageSize)
	{
		var dataLength = imageSize.X * imageSize.Y;
		if (dataLength > MaximumImageDataLength)
			MaximumImageDataLength = dataLength;
	}

	private void ProcessImages()
	{
		while (_pendingImages.TryDequeue(out var data))
		{
			OnImagesCountChanged();
			var buffer = ConvertedPixelsBuffer;
			try
			{
				var buffer2D = buffer.AsSpan().AsSpan2D(data.ImageSize.Y, data.ImageSize.X);
				_pixelConverter.Convert(data.Data2D, buffer2D);
				_imageDataAccess.CreateImage(data.Set, buffer2D, data.CreationTimestamp);
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