using SightKeeper.Application;

namespace SightKeeper.Services.Windows;

public sealed class WindowsMouseMover : MouseMover
{
    public void Move(short x, short y)
    {
        User32.MouseMove(x, y);
    }
}