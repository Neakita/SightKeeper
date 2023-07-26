using System;
using System.Reactive.Subjects;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Elements;

public sealed class ScreenshoterViewModel : ViewModel
{
    public IObservable<bool> IsEnabledChanged => _isEnabledChanged;

    public Model? Model
    {
        get => _screenshoter.Model;
        set => SetProperty(_screenshoter.Model, value, model =>
        {
            _screenshoter.Model = model;
            OnPropertyChanged(nameof(CanToggleIsEnabled));
        });
    }

    public bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            _isEnabled = value;
            _isEnabledChanged.OnNext(value);
            UpdateScreenshoterIsEnabled();
            OnPropertyChanged();
        }
    }

    public bool IsSuspended
    {
        get => _isSuspended;
        set
        {
            if (_isSuspended == value)
                return;
            _isSuspended = value;
            UpdateScreenshoterIsEnabled();
            OnPropertyChanged();
        }
    }

    public bool CanToggleIsEnabled => Model != null;

    public byte? ScreenshotsPerSecond
    {
        get => _screenshoter.ScreenshotsPerSecond;
        set => SetProperty(_screenshoter.ScreenshotsPerSecond, value, framesPerSecond => _screenshoter.ScreenshotsPerSecond = framesPerSecond ?? 0);
    }

    public ushort? MaxImages
    {
        get => _screenshoter.MaxImages;
        set => SetProperty(_screenshoter.MaxImages, value, maxImages => _screenshoter.MaxImages = maxImages);
    }

    public ScreenshoterViewModel(Screenshoter screenshoter)
    {
        _screenshoter = screenshoter;
    }

    private readonly Screenshoter _screenshoter;
    private readonly Subject<bool> _isEnabledChanged = new();

    private bool _isEnabled;
    private bool _isSuspended = true;

    private void UpdateScreenshoterIsEnabled() =>
        _screenshoter.IsEnabled = IsEnabled && !IsSuspended;
}