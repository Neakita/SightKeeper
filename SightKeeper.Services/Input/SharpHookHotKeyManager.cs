using System.Reactive.Disposables;
using System.Reactive.Linq;
using SharpHook;
using SharpHook.Native;
using SightKeeper.Commons;

namespace SightKeeper.Services.Input;

public sealed class SharpHookHotKeyManager(ReactiveGlobalHookFacade hook)
{
    public IDisposable Register(KeyCode key, Action<HotKey> onPressedAction)
    {
        var hotKey = GetHotKey(key);
        var disposable = hotKey.Pressed.Subscribe(onPressedAction);
        return disposable;
    }
    
    public IDisposable Register(KeyCode key, Action onPressedAction, Action onReleasedAction)
    {
        var hotKey = GetHotKey(key);
        CompositeDisposable disposable = new();
        disposable.Add(hotKey.Pressed.Subscribe(_ => onPressedAction()));
        disposable.Add(hotKey.Released.Subscribe(_ => onReleasedAction()));
        return disposable;
    }
    
    public IDisposable Register(MouseButton button, Action<HotKey> onPressedAction)
    {
        var hotKey = GetHotKey(button);
        var disposable = hotKey.Pressed.Subscribe(onPressedAction);
        return disposable;
    }
    
    public IDisposable Register(MouseButton button, Action onPressedAction, Action onReleasedAction)
    {
        var hotKey = GetHotKey(button);
        CompositeDisposable disposable = new();
        disposable.Add(hotKey.Pressed.Subscribe(_ => onPressedAction()));
        disposable.Add(hotKey.Released.Subscribe(_ => onReleasedAction()));
        return disposable;
    }

    public IDisposable Register(MouseButton button, Action onPressedAction)
    {
        var hotKey = GetHotKey(button);
        CompositeDisposable disposable = new();
        disposable.Add(hotKey.Pressed.Subscribe(_ => onPressedAction()));
        return disposable;
    }

    private readonly Dictionary<KeyCode, HotKey> _keyboardHotKeys = new();
    private readonly Dictionary<MouseButton, HotKey> _mouseButtonHotKeys = new();
    private CompositeDisposable? _sessionDisposables;

    private HotKey GetHotKey(KeyCode key)
    {
        if (_keyboardHotKeys.TryGetValue(key, out var hotKey))
            return hotKey;
        return CreateHotKey(key);
    }

    private HotKey GetHotKey(MouseButton button)
    {
        if (_mouseButtonHotKeys.TryGetValue(button, out var hotKey))
            return hotKey;
        return CreateHotKey(button);
    }

    private HotKey CreateHotKey(KeyCode key)
    {
        var hotKey = new HotKey(new KeyGesture(key));
        _keyboardHotKeys.Add(key, hotKey);
        hotKey.HasObserversObservable.SkipWhile(hasObservers => !hasObservers)
            .FirstAsync(hasObservers => !hasObservers).Subscribe(_ => ReleaseHotKey(key));
        EnsureSubscribedOnEvents();
        return hotKey;
    }

    private HotKey CreateHotKey(MouseButton button)
    {
        var hotKey = new HotKey(new MouseGesture(button));
        _mouseButtonHotKeys.Add(button, hotKey);
        hotKey.HasObserversObservable.SkipWhile(hasObservers => !hasObservers)
            .FirstAsync(hasObservers => !hasObservers).Subscribe(_ => ReleaseHotKey(button));
        EnsureSubscribedOnEvents();
        return hotKey;
    }
    
    private void EnsureSubscribedOnEvents()
    {
        if (_sessionDisposables != null)
            return;
        CompositeDisposable disposables = new();
        hook.KeyPressed.Subscribe(OnKeyPressed).DisposeWithEx(disposables);
        hook.KeyReleased.Subscribe(OnKeyReleased).DisposeWithEx(disposables);
        hook.MousePressed.Subscribe(OnMousePressed).DisposeWithEx(disposables);
        hook.MouseReleased.Subscribe(OnMouseReleased).DisposeWithEx(disposables);
        _sessionDisposables = disposables;
    }

    private void ReleaseHotKey(KeyCode key)
    {
        _keyboardHotKeys.Remove(key);
        UnSubscribeFromEventsIfSuitable();
    }
    
    private void ReleaseHotKey(MouseButton button)
    {
        _mouseButtonHotKeys.Remove(button);
        UnSubscribeFromEventsIfSuitable();
    }

    private void UnSubscribeFromEventsIfSuitable()
    {
        if (_sessionDisposables == null || _keyboardHotKeys.Count != 0 || _mouseButtonHotKeys.Count != 0)
            return;
        _sessionDisposables.Dispose();
        _sessionDisposables = null;
    }

    private void OnKeyPressed(KeyboardHookEventArgs args)
    {
        if (!_keyboardHotKeys.TryGetValue(args.Data.KeyCode, out var hotKey))
            return;
        hotKey.IsPressed = true;
        hotKey.NotifyPressed();
    }

    private void OnKeyReleased(KeyboardHookEventArgs args)
    {
        if (!_keyboardHotKeys.TryGetValue(args.Data.KeyCode, out var hotKey))
            return;
        hotKey.IsPressed = false;
        hotKey.NotifyReleased();
    }

    private void OnMousePressed(MouseHookEventArgs args)
    {
        if (!_mouseButtonHotKeys.TryGetValue(args.Data.Button, out var hotKey))
            return;
        hotKey.IsPressed = true;
        hotKey.NotifyPressed();
    }

    private void OnMouseReleased(MouseHookEventArgs args)
    {
        if (!_mouseButtonHotKeys.TryGetValue(args.Data.Button, out var hotKey))
            return;
        hotKey.IsPressed = false;
        hotKey.NotifyReleased();
    }
}