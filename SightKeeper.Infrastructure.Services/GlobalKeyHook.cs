using SharpHook;
using SightKeeper.Application;

namespace SightKeeper.Infrastructure.Services;

public sealed class GlobalKeyHook : KeyHook
{
	public GlobalKeyHook()
	{
		TaskPoolGlobalHook hook = new();
		hook.KeyPressed += HookOnKeyPressed;
		hook.KeyReleased += HookOnKeyReleased;
		hook.Run();
	}

	private void HookOnKeyPressed(object? sender, KeyboardHookEventArgs e) => KeyPressed?.Invoke(e.Data.KeyCode);

	private void HookOnKeyReleased(object? sender, KeyboardHookEventArgs e) => KeyReleased?.Invoke(e.Data.KeyCode);

	public event KeyHook.KeyHandler? KeyPressed;
	public event KeyHook.KeyHandler? KeyReleased;
}