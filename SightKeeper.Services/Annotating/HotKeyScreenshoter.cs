using CommunityToolkit.Diagnostics;
using SharpHook.Native;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model;
using SightKeeper.Services.Input;

namespace SightKeeper.Services.Annotating;

public sealed class HotKeyScreenshoter : StreamModelScreenshoter
{
    public Model? Model
    {
        get => _screenshoter.Model;
        set
        {
            Guard.IsFalse(IsEnabled);
            _screenshoter.Model = value;
        }
    }

    public bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            if (_isEnabled == value)
                return;
            Guard.IsNotNull(Model);
            _isEnabled = value;
            if (value)
                Enable();
            else
                Disable();
        }
    }

    public byte ScreenshotsPerSecond
    {
        get => _framesPerSecond;
        set
        {
            _framesPerSecond = value;
            if (value == 0)
                _timeout = null;
            else
                _timeout = 1000 / value;
        }
    }

    public HotKeyScreenshoter(HotKeyManager hotKeyManager, ModelScreenshoter screenshoter)
    {
        _hotKeyManager = hotKeyManager;
        _screenshoter = screenshoter;
        ScreenshotsPerSecond = 1;
    }

    private readonly HotKeyManager _hotKeyManager;
    private readonly ModelScreenshoter _screenshoter;

    private IDisposable? _disposable;
    private bool _isEnabled;
    private byte _framesPerSecond;
    private int? _timeout;

    private void Enable() => _disposable = _hotKeyManager.Register(MouseButton.Button1, OnHotKeyPressed);
    private void Disable()
    {
        Guard.IsNotNull(_disposable);
        _disposable.Dispose();
    }

    private void OnHotKeyPressed(HotKey hotKey)
    {
        lock (this)
        {
            while (hotKey.IsPressed)
            {
                _screenshoter.MakeScreenshot();
                if (_timeout == null)
                    break;
                Thread.Sleep(_timeout.Value);
            }
        }
    }
}