using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace SightKeeper.Application.Windows;

public sealed class WindowsMouseMover : MouseMover
{
    public IObservable<(int xDelta, int yDelta)> Moved => _moved.AsObservable();

    public void Move(int xDelta, int yDelta)
    {
        User32.MouseMove(xDelta, yDelta);
        _moved.OnNext((xDelta, yDelta));
    }

    private readonly Subject<(int, int)> _moved = new();
}