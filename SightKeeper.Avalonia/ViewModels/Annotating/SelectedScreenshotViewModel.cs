using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
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
        DetectorItems.Connect()
            .Transform(detectorItem => (DrawerItem)detectorItem)
            .PopulateInto(_items)
            .DisposeWithEx(_disposable);
        DetectedItems.Connect()
            .Transform(detectedItem => (DrawerItem)detectedItem)
            .PopulateInto(_items)
            .DisposeWithEx(_disposable);
        _items.Connect()
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
    private readonly SourceList<DrawerItem> _items = new();
    private int? _selectedScreenshotIndex;
}