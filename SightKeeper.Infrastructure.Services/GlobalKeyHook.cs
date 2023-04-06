using SharpHook;
using SharpHook.Native;
using SightKeeper.Application;

namespace SightKeeper.Infrastructure.Services;

public sealed class GlobalKeyHook : KeyHook, IDisposable
{
	public GlobalKeyHook()
	{
		_hook = new TaskPoolGlobalHook();
		_hook.KeyPressed += OnKeyPressed;
		_hook.KeyReleased += OnKeyReleased;
		_hook.MousePressed += HookOnMousePressed;
		_hook.MouseReleased += HookOnMouseReleased;
		_hook.MouseClicked += HookOnMouseClicked;
		_hook.RunAsync();
	}

	private void HookOnMouseClicked(object? sender, MouseHookEventArgs e)
	{
		
	}

	private void OnKeyPressed(object? sender, KeyboardHookEventArgs e)
	{
		KeyCode key = e.Data.KeyCode;
		_pressedKeys.Add(key);
		Task.Run(() => KeyPressed?.Invoke(this, key));
	}

	private void OnKeyReleased(object? sender, KeyboardHookEventArgs e)
	{
		KeyCode key = e.Data.KeyCode;
		_pressedKeys.Remove(key);
		Task.Run(() => KeyReleased?.Invoke(this, key));
	}

	private void HookOnMousePressed(object? sender, MouseHookEventArgs e)
	{
		MouseButton button = e.Data.Button;
		_pressedMouseButtons.Add(button);
		Task.Run(() => MouseButtonPressed?.Invoke(this, button));
	}

	private void HookOnMouseReleased(object? sender, MouseHookEventArgs e)
	{
		MouseButton button = e.Data.Button;
		_pressedMouseButtons.Remove(button);
		Task.Run(() => MouseButtonReleased?.Invoke(this, button));
	}


	public event KeyHook.KeyHandler? KeyPressed;
	public event KeyHook.KeyHandler? KeyReleased;
	public event KeyHook.MouseButtonHandler? MouseButtonPressed;
	public event KeyHook.MouseButtonHandler? MouseButtonReleased;
	
	public bool IsPressed(KeyCode key) => _pressedKeys.Contains(key);
	public bool IsPressed(MouseButton button) => _pressedMouseButtons.Contains(button);

	public void Dispose()
	{
		if (_hook.IsDisposed) return;
		_hook.Dispose();
	}
	
	private readonly TaskPoolGlobalHook _hook;
	private readonly HashSet<KeyCode> _pressedKeys = new();
	private readonly HashSet<MouseButton> _pressedMouseButtons = new();
}