using System.Reactive.Linq;
using System.Reactive.Subjects;
using SightKeeper.Application;

namespace SightKeeper.Services.Windows;

public sealed class WindowsMouseMover : MouseMover
{
    public IObservable<(int xDelta, int yDelta)> Moved => _moved.AsObservable();

    public void Move(int xDelta, int yDelta)
    {
        User32.MouseMove(xDelta, yDelta);
        _moved.OnNext((xDelta, yDelta));
    }

    public void SetPosition(int x, int y)
    {
        Cursor.Position = new Point(x, y);
    }

    private readonly Subject<(int, int)> _moved = new();
}