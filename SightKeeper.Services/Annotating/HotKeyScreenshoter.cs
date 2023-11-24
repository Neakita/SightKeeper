using CommunityToolkit.Diagnostics;
using Serilog;
using SharpHook.Native;
using SightKeeper.Application.Annotating;
using SightKeeper.Data;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;
using SightKeeper.Services.Input;

namespace SightKeeper.Services.Annotating;

public sealed class HotKeyScreenshoter : StreamDataSetScreenshoter
{
    public DataSet? DataSet
    {
        get => _screenshoter.DataSet;
        set
        {
            Guard.IsFalse(IsEnabled);
            _screenshoter.DataSet = value;
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
                _timeout = null;
            else
                _timeout = 1000 / value;
        }
    }

    public HotKeyScreenshoter(SharpHookHotKeyManager sharpHookHotKeyManager, DataSetScreenshoter screenshoter, ScreenshotsDataAccess librariesDataAccess, AppDbContext dbContext)
    {
        _sharpHookHotKeyManager = sharpHookHotKeyManager;
        _screenshoter = screenshoter;
        _librariesDataAccess = librariesDataAccess;
        _dbContext = dbContext;
        ScreenshotsPerSecond = 1;
    }

    private readonly SharpHookHotKeyManager _sharpHookHotKeyManager;
    private readonly DataSetScreenshoter _screenshoter;
    private readonly ScreenshotsDataAccess _librariesDataAccess;
    private readonly AppDbContext _dbContext;

    private IDisposable? _disposable;
    private bool _isEnabled;
    private byte _framesPerSecond;
    private int? _timeout;

    private void Enable() => _disposable = _sharpHookHotKeyManager.Register(MouseButton.Button1, OnHotKeyPressed);
    private void Disable()
    {
        Guard.IsNotNull(_disposable);
        _disposable.Dispose();
    }

    private void OnHotKeyPressed(HotKey hotKey)
    {
        Guard.IsNotNull(DataSet);
        var somethingScreenshoted = false;
        while (hotKey.IsPressed)
        {
            if (!_screenshoter.CanMakeScreenshot(out var message))
                Log.Verbose("Can't make screenshot: {Message}", message);
            else
            {
                _screenshoter.MakeScreenshot();
                somethingScreenshoted = true;
            }
            if (_timeout == null)
                break;
            Thread.Sleep(_timeout.Value);
        }
        if (somethingScreenshoted)
            _librariesDataAccess.SaveChanges(DataSet.ScreenshotsLibrary);
    }
}