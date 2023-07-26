using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using SharpHook.Native;
using SightKeeper.Application;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model;
using SightKeeper.Services.Input;

namespace SightKeeper.Services.Annotating;

public sealed class HotKeyScreenshoter : Screenshoter
{
    public IObservable<Screenshot> Screenshoted => _screenshoted.AsObservable();
    public IObservable<Screenshot> ScreenshotRemoved => _removed.AsObservable();

    public Model? Model
    {
        get => _model;
        set
        {
            Guard.IsFalse(IsEnabled);
            _model = value;
            _screenCapture.Resolution = _model?.Resolution;
            _screenCapture.Game = _model?.Game;
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

    public ushort? MaxImages { get; set; } = 500;

    public HotKeyScreenshoter(HotKeyManager hotKeyManager, ScreenCapture screenCapture)
    {
        _hotKeyManager = hotKeyManager;
        _screenCapture = screenCapture;
        ScreenshotsPerSecond = 1;
    }

    private readonly HotKeyManager _hotKeyManager;
    private readonly ScreenCapture _screenCapture;
    private readonly Subject<Screenshot> _screenshoted = new();
    private readonly Subject<Screenshot> _removed = new();

    private IDisposable? _disposable;
    private bool _isEnabled;
    private byte _framesPerSecond;
    private int? _timeout;
    private Model? _model;

    private void Enable() => _disposable = _hotKeyManager.Register(MouseButton.Button1, OnHotKeyPressed);
    private void Disable() => _disposable?.Dispose();
    private void OnHotKeyPressed(HotKey hotKey)
    {
        lock (this)
        {
            while (hotKey.IsPressed)
            {
                Screenshot();
                if (_timeout == null)
                    break;
                Thread.Sleep(_timeout.Value);
            }
            ClearExceedScreenshots(); 
        }
    }
    private void Screenshot()
    {
        Guard.IsNotNull(Model);
        var image = _screenCapture.Capture();
        var screenshot = Model.ScreenshotsLibrary.CreateScreenshot(image);
        _screenshoted.OnNext(screenshot);
    }
    private void ClearExceedScreenshots()
    {
        if (MaxImages == null)
            return;
        Guard.IsNotNull(Model);
        var screenshotsToDelete = Model.ScreenshotsLibrary.Screenshots
            .OrderByDescending(screenshot => screenshot.CreationDate)
            .Skip(MaxImages.Value)
            .ToList();
        foreach (var screenshot in screenshotsToDelete)
        {
            Model.ScreenshotsLibrary.DeleteScreenshot(screenshot);
            _removed.OnNext(screenshot);
        }
    }
}