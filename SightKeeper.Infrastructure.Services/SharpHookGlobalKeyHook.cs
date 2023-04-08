using SharpHook;
using SharpHook.Native;
using SightKeeper.Application;
using SightKeeper.Infrastructure.Common;

namespace SightKeeper.Infrastructure.Services;

public sealed class SharpHookGlobalKeyHook : KeyHook, IDisposable
{
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


	private event KeyHook.KeyHandler? KeyPressed;

	event KeyHook.KeyHandler? KeyHook.KeyPressed
	{
		add
		{
			KeyPressed += value;
			UpdateHookState();
		}
		remove
		{
			KeyPressed -= value;
			UpdateHookState();
		}
	}

	private event KeyHook.KeyHandler? KeyReleased;

	event KeyHook.KeyHandler? KeyHook.KeyReleased
	{
		add
		{
			KeyReleased += value;
			UpdateHookState();
		}
		remove
		{
			KeyReleased -= value;
			UpdateHookState();
		}
	}

	private event KeyHook.MouseButtonHandler? MouseButtonPressed;

	event KeyHook.MouseButtonHandler? KeyHook.MouseButtonPressed
	{
		add
		{
			MouseButtonPressed += value;
			UpdateHookState();
		}
		remove
		{
			MouseButtonPressed -= value;
			UpdateHookState();
		}
	}

	private event KeyHook.MouseButtonHandler? MouseButtonReleased;

	event KeyHook.MouseButtonHandler? KeyHook.MouseButtonReleased
	{
		add
		{
			MouseButtonReleased += value;
			UpdateHookState();
		}
		remove
		{
			MouseButtonReleased -= value;
			UpdateHookState();
		}
	}

	public bool IsPressed(KeyCode key) => _pressedKeys.Contains(key);
	public bool IsPressed(MouseButton button) => _pressedMouseButtons.Contains(button);

	public void Dispose()
	{
		if (_hook.IsDisposed) return;
		_hook.Dispose();
	}

	private bool IsRunning => _hook != null;
	private readonly HashSet<KeyCode> _pressedKeys = new();
	private readonly HashSet<MouseButton> _pressedMouseButtons = new();
	private TaskPoolGlobalHook? _hook;

	private void UpdateHookState()
	{
		bool shouldHookBeEnabled =
			KeyPressed != null ||
			KeyReleased != null ||
			MouseButtonPressed != null ||
			MouseButtonReleased != null;
		if (shouldHookBeEnabled && !IsRunning) EnableHook();
		else if (!shouldHookBeEnabled && IsRunning) DisableHook();
	}

	private void EnableHook()
	{
		_hook.ThrowIfNotNull(nameof(_hook));
		_hook = new TaskPoolGlobalHook();
		_hook.KeyPressed += OnKeyPressed;
		_hook.KeyReleased += OnKeyReleased;
		_hook.MousePressed += HookOnMousePressed;
		_hook.MouseReleased += HookOnMouseReleased;
		_hook.RunAsync();
	}

	private void DisableHook()
	{
		_hook.ThrowIfNull(nameof(_hook));
		_hook!.KeyPressed -= OnKeyPressed;
		_hook.KeyReleased -= OnKeyReleased;
		_hook.MousePressed -= HookOnMousePressed;
		_hook.MouseReleased -= HookOnMouseReleased;
		_hook.Dispose();
		_hook = null;
	}
}