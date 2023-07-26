using SharpHook.Native;

namespace SightKeeper.Services.Input;

public sealed class KeyGesture : Gesture
{
    public KeyCode Key { get; }

    public KeyGesture(KeyCode key)
    {
        Key = key;
    }

    public override string ToString() =>
        Key.ToStringEx();
}