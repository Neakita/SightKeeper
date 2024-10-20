using System.Buffers;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Screenshotting.Saving;

public sealed class BufferedScreenshotsSaver<TPixel> : ScreenshotsSaver<TPixel>, PendingScreenshotsCountReporter, IDisposable
	where TPixel : unmanaged, IPixel<TPixel>
{
	public override Vector2<ushort> ImageSize
	{
		get => _imageSize;
		set
		{
			if (_imageSize == value)
				return;
			Guard.IsGreaterThan<ushort>(value.X, 0);
			Guard.IsGreaterThan<ushort>(value.Y, 0);
			_imageSize = value;
			var arrayPool = CreateArrayPool();
			_arrayPool.SetTarget(arrayPool);
			foreach (var session in _sessions.Values)
				session.ArrayPool = arrayPool;
		}
	}

	public BehaviorObservable<ushort> PendingScreenshotsCount => _pendingScreenshotsCount;

	public BufferedScreenshotsSaver(ScreenshotsDataAccess screenshotsDataAccess)
	{
		_arrayPool = new WeakReference<ArrayPool<TPixel>>(null!);
		_screenshotsDataAccess = screenshotsDataAccess;
		ImageSize = new Vector2<ushort>(320, 320);
	}

	public override ScreenshotsSaverSession<TPixel> AcquireSession(ScreenshotsLibrary library)
	{
		if (_sessions.TryGetValue(library, out var session))
		{
			if (_freeSessions.Remove(session, out var subscription))
				subscription.Dispose();
			return session;
		}
		session = new BufferedScreenshotsSaverSession<TPixel>(library, _screenshotsDataAccess, ArrayPool);
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
	private readonly BehaviorSubject<ushort> _pendingScreenshotsCount = new(0);
	private readonly Dictionary<ScreenshotsLibrary, BufferedScreenshotsSaverSession<TPixel>> _sessions = new();
	private readonly Dictionary<BufferedScreenshotsSaverSession<TPixel>, IDisposable> _freeSessions = new();
	private ArrayPool<TPixel> ArrayPool
	{
		get
		{
			if (_arrayPool.TryGetTarget(out var arrayPool))
				return arrayPool;
			arrayPool = CreateArrayPool();
			_arrayPool.SetTarget(arrayPool);
			return arrayPool;
		}
	}
	private readonly WeakReference<ArrayPool<TPixel>> _arrayPool;
	private Vector2<ushort> _imageSize;
	private IDisposable _aggregateSubscription = Disposable.Empty;

	private ArrayPool<TPixel> CreateArrayPool()
	{
		return ArrayPool<TPixel>.Create(ImageSize.X * ImageSize.Y, 50);
	}

	private void UpdateAggregateSubscription()
	{
		_aggregateSubscription.Dispose();
		_aggregateSubscription = _sessions
			.Select(session => session.Value.PendingScreenshotsCount)
			.CombineLatest(counts => (ushort)counts.Sum(count => count))
			.Subscribe(_pendingScreenshotsCount);
	}
}