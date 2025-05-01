using System.Buffers;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.ScreenCapturing.Saving;

public sealed class BufferedImageSaver<TPixel> : ImageSaver<TPixel>, PendingImagesCountReporter, IDisposable
{
	public override Vector2<ushort> MaximumImageSize
	{
		get;
		set
		{
			if (field == value)
				return;
			Guard.IsGreaterThan<ushort>(value.X, 0);
			Guard.IsGreaterThan<ushort>(value.Y, 0);
			field = value;
			UpdateArrayPools();
		}
	}

	public BehaviorObservable<ushort> PendingImagesCount => _pendingImagesCount;

	public BufferedImageSaver(ImageDataAccess imageDataAccess, PixelConverter<TPixel, Rgba32> pixelConverter, ImagesCleaner imagesCleaner)
	{
		_rawPixelsArrayPool = new WeakReference<ArrayPool<TPixel>>(null!);
		_convertedPixelsArrayPool = new WeakReference<ArrayPool<Rgba32>>(null!);
		_imageDataAccess = imageDataAccess;
		_pixelConverter = pixelConverter;
		_imagesCleaner = imagesCleaner;
		MaximumImageSize = new Vector2<ushort>(320, 320);
	}

	public override ImageSaverSession<TPixel> AcquireSession(ImageSet set)
	{
		if (_sessions.TryGetValue(set, out var session))
		{
			if (_freeSessions.Remove(session, out var subscription))
				subscription.Dispose();
			return session;
		}
		session = new BufferedImageSaverSession<TPixel>(set, _imageDataAccess, RawPixelsArrayPool, ConvertedPixelsArrayPool, _pixelConverter, _imagesCleaner);
		_sessions.Add(set, session);
		UpdateAggregateSubscription();
		return session;
	}

	public override void ReleaseSession(ImageSaverSession<TPixel> session)
	{
		var bufferedSession = (BufferedImageSaverSession<TPixel>)session;
		if (bufferedSession.PendingImagesCount.Value == 0)
		{
			Guard.IsTrue(_sessions.Remove(bufferedSession.Set));
			bufferedSession.Dispose();
			UpdateAggregateSubscription();
			return;
		}
		var subscription = bufferedSession.PendingImagesCount.Subscribe(count =>
		{
			if (count != 0)
				return;
			Guard.IsTrue(_sessions.Remove(bufferedSession.Set));
			Guard.IsTrue(_freeSessions.Remove(bufferedSession, out var subscription));
			bufferedSession.Dispose();
			subscription.Dispose();
			UpdateAggregateSubscription();
		});
		_freeSessions.Add(bufferedSession, subscription);
	}

	public override void Dispose()
	{
		Guard.IsEqualTo(_sessions.Count, 0);
		_pendingImagesCount.Dispose();
	}

	private readonly ImageDataAccess _imageDataAccess;
	private readonly PixelConverter<TPixel, Rgba32> _pixelConverter;
	private readonly ImagesCleaner _imagesCleaner;
	private readonly BehaviorSubject<ushort> _pendingImagesCount = new(0);
	private readonly Dictionary<ImageSet, BufferedImageSaverSession<TPixel>> _sessions = new();
	private readonly Dictionary<BufferedImageSaverSession<TPixel>, IDisposable> _freeSessions = new();
	private readonly WeakReference<ArrayPool<TPixel>> _rawPixelsArrayPool;
	private readonly WeakReference<ArrayPool<Rgba32>> _convertedPixelsArrayPool;
	private IDisposable _aggregateSubscription = Disposable.Empty;
	private ArrayPool<TPixel> RawPixelsArrayPool
	{
		get
		{
			if (_rawPixelsArrayPool.TryGetTarget(out var arrayPool))
				return arrayPool;
			arrayPool = CreateArrayPool<TPixel>();
			_rawPixelsArrayPool.SetTarget(arrayPool);
			return arrayPool;
		}
	}
	private ArrayPool<Rgba32> ConvertedPixelsArrayPool
	{
		get
		{
			if (_convertedPixelsArrayPool.TryGetTarget(out var arrayPool))
				return arrayPool;
			arrayPool = CreateArrayPool<Rgba32>();
			_convertedPixelsArrayPool.SetTarget(arrayPool);
			return arrayPool;
		}
	}

	private ArrayPool<T> CreateArrayPool<T>()
	{
		return ArrayPool<T>.Create(MaximumImageSize.X * MaximumImageSize.Y, 50);
	}

	private void UpdateAggregateSubscription()
	{
		_aggregateSubscription.Dispose();
		_aggregateSubscription = _sessions
			.Select(session => session.Value.PendingImagesCount)
			.CombineLatest(counts => (ushort)counts.Sum(count => count))
			.Subscribe(_pendingImagesCount);
	}

	private void UpdateArrayPools()
	{
		var rawPixelsArrayPool = CreateArrayPool<TPixel>();
		var convertedPixelsArrayPool = CreateArrayPool<Rgba32>();
		_rawPixelsArrayPool.SetTarget(rawPixelsArrayPool);
		_convertedPixelsArrayPool.SetTarget(convertedPixelsArrayPool);
		foreach (var session in _sessions.Values)
		{
			session.RawPixelsArrayPool = rawPixelsArrayPool;
			session.ConvertedPixelsArrayPool = convertedPixelsArrayPool;
		}
	}
}