using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Application.Input;

public sealed class HotKey(Gesture.Gesture gesture)
{
    public Gesture.Gesture Gesture { get; } = gesture;
    public bool IsPressed { get; internal set; }

    public void WaitForRelease()
    {
        Guard.IsNotNull(_releaseCompletionSource);
        _releaseCompletionSource.Task.Wait();
    }

    public Task WaitForReleaseAsync(CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(_releaseCompletionSource);
        return _releaseCompletionSource.Task;
    }

    internal IObservable<HotKey> Pressed => _pressed.ObserveOn(TaskPoolScheduler.Default).AsObservable();
    internal IObservable<Unit> Released => _released.ObserveOn(TaskPoolScheduler.Default).AsObservable();

    internal IObservable<bool> HasObserversObservable => _pressed.HasObserversObservable
        .CombineLatest(_released.HasObserversObservable, (a, b) => a || b).DistinctUntilChanged();

    internal void NotifyPressed()
    {
        Guard.IsNull(_releaseCompletionSource);
        _releaseCompletionSource = new TaskCompletionSource();
        _pressed.OnNext(this);
    }

    internal void NotifyReleased()
    {
        Guard.IsNotNull(_releaseCompletionSource);
        _releaseCompletionSource.SetResult();
        _releaseCompletionSource = null;
        _released.OnNext(Unit.Default);
    }

    private readonly ObservableSubject<HotKey> _pressed = new();
    private readonly ObservableSubject<Unit> _released = new();

    private TaskCompletionSource? _releaseCompletionSource;
}