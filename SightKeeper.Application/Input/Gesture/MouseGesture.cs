using SharpHook.Native;

namespace SightKeeper.Application.Input.Gesture;

public sealed class MouseGesture : Gesture
{
    public MouseButton Button { get; }

    public MouseGesture(MouseButton button)
    {
        Button = button;
    }

    public override string ToString() =>
        Button.ToString();
}