using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using SightKeeper.Commons;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed class SelectedScreenshotViewModel : ValueViewModel<ScreenshotViewModel?>
{
    public int? SelectedScreenshotIndex
    {
        get => _selectedScreenshotIndex;
        set => SetProperty(ref _selectedScreenshotIndex, value);
    }

    public ReadOnlyObservableCollection<DrawerItem> Items { get; }
    
    public SourceList<DetectorItemViewModel> DetectorItems { get; } = new();
    public SourceList<DetectedItemViewModel> DetectedItems { get; } = new();

    public SelectedScreenshotViewModel() : base(null)
    {
        Observable.Merge(
                DetectorItems.Connect().Transform(detectorItem => (DrawerItem)detectorItem),
                DetectedItems.Connect().Transform(detectedItem => (DrawerItem)detectedItem))
            .Bind(out var items)
            .Subscribe()
            .DisposeWithEx(_disposable);
        Items = items;
    }

    protected override void OnValueChanged(ScreenshotViewModel? newValue)
    {
        DetectorItems.Clear();
        DetectedItems.Clear();
    }
    
    private readonly CompositeDisposable _disposable = new();
    private int? _selectedScreenshotIndex;
}