using System;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

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
            OnPropertyChanged(nameof(MaxScreenshotsQuantity));
            OnPropertyChanged(nameof(CanChangeMaxScreenshotsQuantity));
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

    public ushort? MaxScreenshotsQuantity
    {
        get => Model?.ScreenshotsLibrary.MaxQuantity;
        set => SetProperty(Model?.ScreenshotsLibrary.MaxQuantity, value, maxImages =>
        {
            Guard.IsNotNull(Model);
            Model.ScreenshotsLibrary.MaxQuantity = maxImages;
        });
    }

    public bool CanChangeMaxScreenshotsQuantity => Model != null;

    public ScreenshoterViewModel(StreamModelScreenshoter screenshoter)
    {
        _screenshoter = screenshoter;
    }

    private readonly Subject<bool> _isEnabledChanged = new();
    private readonly StreamModelScreenshoter _screenshoter;

    private bool _isEnabled;
    private bool _isSuspended = true;

    private void UpdateScreenshoterIsEnabled() =>
        _screenshoter.IsEnabled = IsEnabled && !IsSuspended;
}