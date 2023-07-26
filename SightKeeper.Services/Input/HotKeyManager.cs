using System.Reactive.Disposables;
using SharpHook;
using SharpHook.Native;
using SharpHook.Reactive;

namespace SightKeeper.Services.Input;

public sealed class HotKeyManager
{
    public HotKeyManager(IReactiveGlobalHook hook)
    {
        hook.KeyPressed.Subscribe(OnKeyPressed);
        hook.KeyReleased.Subscribe(OnKeyReleased);
        hook.MousePressed.Subscribe(OnMousePressed);
        hook.MouseReleased.Subscribe(OnMouseReleased);
    }
    
    public IDisposable Register(KeyCode key, Action<HotKey> onPressedAction)
    {
        if (_keyboardHotKeys.TryGetValue(key, out var existingHotKey))
            return existingHotKey.Pressed.Subscribe(onPressedAction);
        HotKey hotKey = new(new KeyGesture(key));
        _keyboardHotKeys.Add(key, hotKey);
        return hotKey.Pressed.Subscribe(onPressedAction);
    }
    
    public IDisposable Register(KeyCode key, Action onPressedAction, Action onReleasedAction)
    {
        CompositeDisposable disposable = new();
        if (_keyboardHotKeys.TryGetValue(key, out var existingHotKey))
        {
            disposable.Add(existingHotKey.Pressed.Subscribe(_ => onPressedAction()));
            disposable.Add(existingHotKey.Released.Subscribe(_ => onReleasedAction()));
        }
        HotKey hotKey = new(new KeyGesture(key));
        _keyboardHotKeys.Add(key, hotKey);
        disposable.Add(hotKey.Pressed.Subscribe(_ => onPressedAction()));
        disposable.Add(hotKey.Released.Subscribe(_ => onReleasedAction()));
        return disposable;
    }
    
    public IDisposable Register(MouseButton button, Action<HotKey> onPressedAction)
    {
        if (_mouseButtonHotKeys.TryGetValue(button, out var existingHotKey))
            return existingHotKey.Pressed.Subscribe(onPressedAction);
        HotKey hotKey = new(new MouseGesture(button));
        _mouseButtonHotKeys.Add(button, hotKey);
        return hotKey.Pressed.Subscribe(onPressedAction);
    }
    
    public IDisposable Register(MouseButton button, Action onPressedAction, Action onReleasedAction)
    {
        CompositeDisposable disposable = new();
        if (_mouseButtonHotKeys.TryGetValue(button, out var existingHotKey))
        {
            disposable.Add(existingHotKey.Pressed.Subscribe(_ => onPressedAction()));
            disposable.Add(existingHotKey.Released.Subscribe(_ => onReleasedAction()));
        }
        HotKey hotKey = new(new MouseGesture(button));
        _mouseButtonHotKeys.Add(button, hotKey);
        disposable.Add(hotKey.Pressed.Subscribe(_ => onPressedAction()));
        disposable.Add(hotKey.Released.Subscribe(_ => onReleasedAction()));
        return disposable;
    }

    private readonly Dictionary<KeyCode, HotKey> _keyboardHotKeys = new();
    private readonly Dictionary<MouseButton, HotKey> _mouseButtonHotKeys = new();

    private void OnKeyPressed(KeyboardHookEventArgs args)
    {
        if (!_keyboardHotKeys.TryGetValue(args.Data.KeyCode, out var hotKey)) return;
        hotKey.IsPressed = true;
        hotKey.InvokeActions();
    }

    private void OnKeyReleased(KeyboardHookEventArgs args)
    {
        if (_keyboardHotKeys.TryGetValue(args.Data.KeyCode, out var hotKey))
            hotKey.IsPressed = false;
    }

    private void OnMousePressed(MouseHookEventArgs args)
    {
        if (!_mouseButtonHotKeys.TryGetValue(args.Data.Button, out var hotKey)) return;
        hotKey.IsPressed = true;
        hotKey.InvokeActions();
    }

    private void OnMouseReleased(MouseHookEventArgs args)
    {
        if (_mouseButtonHotKeys.TryGetValue(args.Data.Button, out var hotKey))
            hotKey.IsPressed = false;
    }
}