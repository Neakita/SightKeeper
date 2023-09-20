using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace SightKeeper.Services.Input;

public sealed class HotKey
{
    public Gesture Gesture { get; }
    public bool IsPressed { get; internal set; }

    public HotKey(Gesture gesture)
    {
        Gesture = gesture;
    }
    
    internal IObservable<HotKey> Pressed => _pressed.ObserveOn(TaskPoolScheduler.Default).AsObservable();
    internal IObservable<Unit> Released => _released.ObserveOn(TaskPoolScheduler.Default).AsObservable();

    internal void NotifyPressed() => _pressed.OnNext(this);
    internal void NotifyReleased() => _released.OnNext(Unit.Default);

    private readonly Subject<HotKey> _pressed = new();
    private readonly Subject<Unit> _released = new();
}