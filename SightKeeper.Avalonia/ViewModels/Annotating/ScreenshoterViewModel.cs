using System;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed class ScreenshoterViewModel : ViewModel
{
    public IObservable<bool> IsEnabledChanged => _isEnabledChanged;

    public DataSet? Model
    {
        get => _screenshoter.Model;
        set
        {
            if (!SetProperty(_screenshoter.Model, value, model => _screenshoter.Model = model))
                return;
            OnPropertyChanged(nameof(CanToggleIsEnabled));
            OnPropertyChanged(nameof(MaxScreenshotsQuantity));
            OnPropertyChanged(nameof(CanChangeMaxScreenshotsQuantity));
        }
    }

    public bool IsEnabled
    {
        get => _screenshoter.IsEnabled;
        set
        {
            if (!SetProperty(_screenshoter.IsEnabled, value, isEnabled => _screenshoter.IsEnabled = isEnabled))
                return;
            _isEnabledChanged.OnNext(value);
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
}