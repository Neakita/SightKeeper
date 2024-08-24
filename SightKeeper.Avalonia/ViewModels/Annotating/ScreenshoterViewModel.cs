using System;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

internal sealed class ScreenshoterViewModel : ViewModel
{
    public IObservable<bool> IsEnabledChanged => _isEnabledChanged;

    public DetectorDataSet? DataSet
    {
        get => _screenshoter.DataSet;
        set
        {
            if (!SetProperty(_screenshoter.DataSet, value, dataSet => _screenshoter.DataSet = dataSet))
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

    public bool CanToggleIsEnabled => DataSet != null;

    public byte? ScreenshotsPerSecond
    {
        get => _screenshoter.ScreenshotsPerSecond;
        set => SetProperty(_screenshoter.ScreenshotsPerSecond, value, framesPerSecond => _screenshoter.ScreenshotsPerSecond = framesPerSecond ?? 0);
    }

    public ushort? MaxScreenshotsQuantity
    {
        get => DataSet?.Screenshots.MaxQuantity;
        set => SetProperty(DataSet?.Screenshots.MaxQuantity, value, maxImages =>
        {
            Guard.IsNotNull(DataSet);
            DataSet.Screenshots.MaxQuantity = maxImages;
        });
    }

    public bool CanChangeMaxScreenshotsQuantity => DataSet != null;

    public ScreenshoterViewModel(StreamScreenshoter screenshoter)
    {
        _screenshoter = screenshoter;
    }

    private readonly Subject<bool> _isEnabledChanged = new();
    private readonly StreamScreenshoter _screenshoter;
}