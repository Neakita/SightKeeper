using SharpHook.Native;
using SightKeeper.Application.Input;

namespace SightKeeper.Services.Input;

public sealed class SharpHookHotKey : HotKey
{
	public static event Action<SharpHookHotKey>? Disposed;
  
	public bool IsPressed
	{
		get => _isPressed;
		set
		{
			if (_disposed) throw new InvalidOperationException("HotKey is disposed");
			_isPressed = value;
			if (_isPressed) Task.Run(() => _onPressedAction?.Invoke(this));
			else Task.Run(() => _onReleasedAction?.Invoke(this));
		}
	}
	
	public MouseButton? Button { get; }
	public KeyCode? Key { get; }
	

	public SharpHookHotKey(KeyCode key, Action<HotKey>? pressed = null, Action<HotKey>? released = null) : this(pressed, released)
	{
		Key = key;
	}
	
	public SharpHookHotKey(MouseButton button, Action<HotKey>? pressed = null, Action<HotKey>? released = null) : this(pressed, released)
	{
		Button = button;
	}

	public void Dispose()
	{
		if (_disposed) return;
		_isPressed = false;
		Disposed?.Invoke(this);
		_disposed = true;
	}
	

	private readonly Action<HotKey>? _onPressedAction;
	private readonly Action<HotKey>? _onReleasedAction;
	private bool _isPressed;
	private bool _disposed;

	private SharpHookHotKey(Action<HotKey>? pressed = null, Action<HotKey>? released = null)
	{
		_onPressedAction = pressed;
		_onReleasedAction = released;
	}
}
