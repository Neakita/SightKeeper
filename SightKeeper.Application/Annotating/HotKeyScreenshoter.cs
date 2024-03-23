using CommunityToolkit.Diagnostics;
using SharpHook.Native;
using SightKeeper.Application.Input;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Application.Annotating;

public sealed class HotKeyScreenshoter : StreamScreenshoter
{
    public DataSet? DataSet
    {
	    get => _dataSet;
        set
        {
            Guard.IsFalse(IsEnabled);
            _dataSet = value;
        }
    }

    public bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            if (_isEnabled == value)
                return;
            Guard.IsNotNull(DataSet);
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
                _delay = null;
            else
                _delay = 1000 / value;
        }
    }

    public HotKeyScreenshoter(SharpHookHotKeyManager sharpHookHotKeyManager, Screenshoter screenshoter)
    {
        _sharpHookHotKeyManager = sharpHookHotKeyManager;
        _screenshoter = screenshoter;
        ScreenshotsPerSecond = 1;
    }

    private readonly SharpHookHotKeyManager _sharpHookHotKeyManager;
    private readonly Screenshoter _screenshoter;

    private DataSet? _dataSet;
    private IDisposable? _disposable;
    private bool _isEnabled;
    private byte _framesPerSecond;
    private int? _delay;

    private void Enable() => _disposable = _sharpHookHotKeyManager.Register(MouseButton.Button1, OnHotKeyPressed);
    private void Disable()
    {
        Guard.IsNotNull(_disposable);
        _disposable.Dispose();
    }

    private void OnHotKeyPressed(HotKey hotKey)
    {
        Guard.IsNotNull(DataSet);
        while (hotKey.IsPressed)
        {
	        _screenshoter.MakeScreenshot(DataSet);
            if (_delay != null)
				Thread.Sleep(_delay.Value);
        }
    }
}