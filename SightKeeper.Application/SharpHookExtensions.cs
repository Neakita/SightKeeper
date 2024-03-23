using SharpHook.Native;

namespace SightKeeper.Application;

public static class SharpHookExtensions
{
    public static string ToStringEx(this KeyCode key)
    {
        var result = key.ToString();
        result = result[2..];
        return result;
    }
}