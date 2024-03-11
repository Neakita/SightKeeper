using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Disposables;
using DynamicData;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed class SelectedScreenshotViewModel : ValueViewModel<ScreenshotViewModel?>
{
    public int? SelectedScreenshotIndex
    {
        get => _selectedScreenshotIndex;
        set => SetProperty(ref _selectedScreenshotIndex, value);
    }
    public IReadOnlyCollection<DrawerItem> Items
    {
        get => _items;
        private set => SetProperty(ref _items, value);
    }
    public SourceList<DetectorItemViewModel> DetectorItems { get; } = new();
    public SourceList<DetectedItemViewModel> DetectedItems { get; } = new();

    public bool ShowDetectorItems
    {
        get => _showDetectorItems;
        set
        {
            if (SetProperty(ref _showDetectorItems, value))
                SetItems();
        }
    }

    public bool ShowDetectedItems
    {
        get => _showDetectedItems;
        set
        {
            if (SetProperty(ref _showDetectedItems, value))
                SetItems();
        }
    }

    public SelectedScreenshotViewModel() : base(null)
    {
        DetectorItems.Connect()
            .Transform(detectorItem => (DrawerItem)detectorItem)
            .Bind(out var detectorItems)
            .Or(DetectedItems.Connect()
                .Transform(detectedItem => (DrawerItem)detectedItem)
                .Bind(out var detectedItems))
            .Bind(out var items)
            .Subscribe()
            .DisposeWithEx(_disposable);
        _bothItems = items;
        _detectorItems = detectorItems;
        _detectedItems = detectedItems;
        SetItems();
    }

    protected override void OnValueChanged(ScreenshotViewModel? newValue)
    {
        DetectorItems.Clear();
        DetectedItems.Clear();
    }
    
    private readonly CompositeDisposable _disposable = new();
    private readonly ReadOnlyObservableCollection<DrawerItem> _bothItems;
    private readonly ReadOnlyObservableCollection<DrawerItem> _detectorItems;
    private readonly ReadOnlyObservableCollection<DrawerItem> _detectedItems;
    private int? _selectedScreenshotIndex;
    private bool _showDetectorItems = true;
    private bool _showDetectedItems = true;
    private IReadOnlyCollection<DrawerItem> _items;

    [MemberNotNull(nameof(_items))]
    private void SetItems()
    {
        _items = null!;
        if (ShowDetectorItems && ShowDetectedItems)
            Items = _bothItems;
        else if (ShowDetectorItems)
            Items = _detectorItems;
        else if (ShowDetectedItems)
            Items = _detectedItems;
        else
            Items = Array.Empty<DrawerItem>();
    }
}