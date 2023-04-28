using SharpHook;
using SharpHook.Native;
using SightKeeper.Application.Input;

namespace SightKeeper.Infrastructure.Services.Input;

public sealed class SharpHookHotKeyManager : HotKeyManager<KeyCode>, HotKeyManager<MouseButton>, IDisposable
{
	public SharpHookHotKeyManager()
	{
		SharpHookHotKey.Disposed += OnHotKeyDisposed;
		_hook = new TaskPoolGlobalHook();
		_hook.KeyPressed += HookOnKeyPressed;
		_hook.KeyReleased += HookOnKeyReleased;
		_hook.MousePressed += HookOnMousePressed;
		_hook.MouseReleased += HookOnMouseReleased;
		_hook.RunAsync();
	}

	public HotKey Register(KeyCode key, Action<HotKey>? pressed = null, Action<HotKey>? released = null)
	{
		SharpHookHotKey hotKey = new(key, pressed, released);
		if (_hotKeysByKeyCodes.TryGetValue(key, out HashSet<SharpHookHotKey>? hotKeys)) hotKeys.Add(hotKey);
		else
		{
			HashSet<SharpHookHotKey> newSet = new() {hotKey};
			_hotKeysByKeyCodes.Add(key, newSet);
		}
		return hotKey;
	}

	public HotKey Register(MouseButton button, Action<HotKey>? pressed = null, Action<HotKey>? released = null)
	{
		SharpHookHotKey hotKey = new(button, pressed, released);
		if (_hotKeysByMouseButtons.TryGetValue(button, out HashSet<SharpHookHotKey>? hotKeys)) hotKeys.Add(hotKey);
		else
		{
			HashSet<SharpHookHotKey> newSet = new() {hotKey};
			_hotKeysByMouseButtons.Add(button, newSet);
		}
		return hotKey;
	}

	public void Dispose()
	{
		_hook.Dispose();
	}

	private readonly Dictionary<KeyCode, HashSet<SharpHookHotKey>> _hotKeysByKeyCodes = new();
	private readonly Dictionary<MouseButton, HashSet<SharpHookHotKey>> _hotKeysByMouseButtons = new();
	private readonly TaskPoolGlobalHook _hook;

	private void OnHotKeyDisposed(SharpHookHotKey hotKey)
	{
		if (hotKey.Key != null) _hotKeysByKeyCodes[hotKey.Key.Value].Remove(hotKey);
		else if (hotKey.Button != null) _hotKeysByMouseButtons[hotKey.Button.Value].Remove(hotKey);
	}

	private void HookOnKeyPressed(object? sender, KeyboardHookEventArgs e)
	{
		KeyCode keyCode = e.Data.KeyCode;
		if (!_hotKeysByKeyCodes.TryGetValue(keyCode, out HashSet<SharpHookHotKey>? hotKeys)) return;
		foreach (SharpHookHotKey hotKey in hotKeys) hotKey.IsPressed = true;
	}
	
	private void HookOnKeyReleased(object? sender, KeyboardHookEventArgs e)
	{
		KeyCode keyCode = e.Data.KeyCode;
		if (!_hotKeysByKeyCodes.TryGetValue(keyCode, out HashSet<SharpHookHotKey>? hotKeys)) return;
		foreach (SharpHookHotKey hotKey in hotKeys) hotKey.IsPressed = false;
	}

	private void HookOnMousePressed(object? sender, MouseHookEventArgs e)
	{
		MouseButton button = e.Data.Button;
		if (!_hotKeysByMouseButtons.TryGetValue(button, out HashSet<SharpHookHotKey>? hotKeys)) return;
		foreach (SharpHookHotKey hotKey in hotKeys) hotKey.IsPressed = true;
	}

	private void HookOnMouseReleased(object? sender, MouseHookEventArgs e)
	{
		MouseButton button = e.Data.Button;
		if (!_hotKeysByMouseButtons.TryGetValue(button, out HashSet<SharpHookHotKey>? hotKeys)) return;
		foreach (SharpHookHotKey hotKey in hotKeys) hotKey.IsPressed = false;
	}
}
