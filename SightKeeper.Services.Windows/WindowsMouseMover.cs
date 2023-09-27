using System.Reactive.Linq;
using System.Reactive.Subjects;
using SightKeeper.Application;

namespace SightKeeper.Services.Windows;

public sealed class WindowsMouseMover : MouseMover
{
    public IObservable<(short xDelta, short yDelta)> Moved => _moved.AsObservable();

    public void Move(short xDelta, short yDelta)
    {
        User32.MouseMove(xDelta, yDelta);
        _moved.OnNext((xDelta, yDelta));
    }

    public void SetPosition(short x, short y)
    {
        Cursor.Position = new Point(x, y);
    }

    private readonly Subject<(short, short)> _moved = new();
}