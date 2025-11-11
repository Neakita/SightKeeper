using System.Reactive.Linq;
using System.Reactive.Subjects;
using SightKeeper.Application.Interop;

namespace SightKeeper.Application.Windows;

internal sealed class WindowsMouseMover : MouseMover
{
    public IObservable<(int xDelta, int yDelta)> Moved => _moved.AsObservable();

    public void Move(int xDelta, int yDelta)
    {
        User32.MouseMove(xDelta, yDelta);
        _moved.OnNext((xDelta, yDelta));
    }

    private readonly Subject<(int, int)> _moved = new();
}