using SharpHook.Native;

namespace SightKeeper.Services;

public static class SharpHookExtensions
{
    public static string ToStringEx(this KeyCode key)
    {
        var result = key.ToString();
        if (key != KeyCode.CharUndefined)
            result = result[2..];
        return result;
    }
}