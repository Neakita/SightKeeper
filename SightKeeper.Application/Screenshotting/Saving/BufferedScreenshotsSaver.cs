using System.Buffers;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Screenshotting.Saving;

public sealed class BufferedScreenshotsSaver<TPixel> : ScreenshotsSaver<TPixel>, PendingScreenshotsCountReporter, IDisposable
{
	public override Vector2<ushort> MaximumImageSize
	{
		get => _imageSize;
		set
		{
			if (_imageSize == value)
				return;
			Guard.IsGreaterThan<ushort>(value.X, 0);
			Guard.IsGreaterThan<ushort>(value.Y, 0);
			_imageSize = value;
			UpdateArrayPools();
		}
	}

	public BehaviorObservable<ushort> PendingScreenshotsCount => _pendingScreenshotsCount;

	public BufferedScreenshotsSaver(ScreenshotsDataAccess screenshotsDataAccess, PixelConverter<TPixel, Rgba32> pixelConverter)
	{
		_rawPixelsArrayPool = new WeakReference<ArrayPool<TPixel>>(null!);
		_convertedPixelsArrayPool = new WeakReference<ArrayPool<Rgba32>>(null!);
		_screenshotsDataAccess = screenshotsDataAccess;
		_pixelConverter = pixelConverter;
		MaximumImageSize = new Vector2<ushort>(320, 320);
	}

	public override ScreenshotsSaverSession<TPixel> AcquireSession(ScreenshotsLibrary library)
	{
		if (_sessions.TryGetValue(library, out var session))
		{
			if (_freeSessions.Remove(session, out var subscription))
				subscription.Dispose();
			return session;
		}
		session = new BufferedScreenshotsSaverSession<TPixel>(library, _screenshotsDataAccess, RawPixelsArrayPool, ConvertedPixelsArrayPool, _pixelConverter);
		_sessions.Add(library, session);
		UpdateAggregateSubscription();
		return session;
	}

	public override void ReleaseSession(ScreenshotsSaverSession<TPixel> session)
	{
		var bufferedSession = (BufferedScreenshotsSaverSession<TPixel>)session;
		if (bufferedSession.PendingScreenshotsCount.Value == 0)
		{
			Guard.IsTrue(_sessions.Remove(bufferedSession.Library));
			bufferedSession.Dispose();
			UpdateAggregateSubscription();
			return;
		}
		var subscription = bufferedSession.PendingScreenshotsCount.Subscribe(count =>
		{
			if (count != 0)
				return;
			Guard.IsTrue(_sessions.Remove(bufferedSession.Library));
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
		_pendingScreenshotsCount.Dispose();
	}

	private readonly ScreenshotsDataAccess _screenshotsDataAccess;
	private readonly PixelConverter<TPixel, Rgba32> _pixelConverter;
	private readonly BehaviorSubject<ushort> _pendingScreenshotsCount = new(0);
	private readonly Dictionary<ScreenshotsLibrary, BufferedScreenshotsSaverSession<TPixel>> _sessions = new();
	private readonly Dictionary<BufferedScreenshotsSaverSession<TPixel>, IDisposable> _freeSessions = new();
	private readonly WeakReference<ArrayPool<TPixel>> _rawPixelsArrayPool;
	private readonly WeakReference<ArrayPool<Rgba32>> _convertedPixelsArrayPool;
	private Vector2<ushort> _imageSize;
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
			.Select(session => session.Value.PendingScreenshotsCount)
			.CombineLatest(counts => (ushort)counts.Sum(count => count))
			.Subscribe(_pendingScreenshotsCount);
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