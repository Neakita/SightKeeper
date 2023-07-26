using SharpHook.Native;

namespace SightKeeper.Services.Input;

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