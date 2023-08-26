using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using Avalonia;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Application.Annotating;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed partial class DrawerViewModel : ViewModel, IDisposable
{
    public const double MinimumDimensionSize = 0.01;

    public DataSetViewModel? DataSetViewModel { get; set; }

    private sealed class DrawingData
    {
        public DetectorItemViewModel ItemViewModel { get; }

        public DrawingData(Point startPosition, DetectorItemViewModel itemViewModel)
        {
            _startPosition = startPosition;
            ItemViewModel = itemViewModel;
        }

        public void UpdateBounding(Point currentPosition) =>
            ItemViewModel.Bounding.SetFromTwoPositions(_startPosition, currentPosition);
        
        private readonly Point _startPosition;
    }
    
    public ScreenshotViewModel? Screenshot => _screenshots.SelectedScreenshot;
    public Asset? Asset => Screenshot?.Item.Asset;
    public ObservableCollection<DetectorItemViewModel> Items { get; } = new();
    public bool CanBeginDrawing => _tools.SelectedItemClass != null && _screenshots.SelectedScreenshot != null;

    public DrawerViewModel(
        AnnotatorScreenshotsViewModel screenshots,
        AnnotatorToolsViewModel tools,
        DetectorAnnotator annotator,
        DetectorItemResizer resizer,
        DetectorAssetsDataAccess assetsDataAccess)
    {
        _resizer = resizer;
        _screenshots = screenshots;
        _tools = tools;
        _annotator = annotator;
        CompositeDisposable disposable = new();
        _disposable = disposable;
        screenshots.SelectedScreenshotChanged.Subscribe(_ =>
        {
            OnPropertyChanged(nameof(Screenshot));
            OnPropertyChanged(nameof(Asset));
            Items.Clear();
            var asset = Asset;
            if (asset == null)
                return;
            assetsDataAccess.LoadItems(asset);
            foreach (var item in asset.Items.Select(item => new DetectorItemViewModel(item, resizer, this)))
                Items.Add(item);
        }).DisposeWith(disposable);
        tools.UnMarkSelectedScreenshotAsAssetExecuted.Subscribe(_ =>
        {
            OnPropertyChanged(nameof(Asset));
            Items.Clear();
        }).DisposeWith(disposable);
        tools.DeleteItemExecuted
            .Subscribe(item => Items.Remove(item))
            .DisposeWith(disposable);
    }
    
    public void BeginDrawing(Point startPosition)
    {
        Guard.IsNull(_drawingData);
        Guard.IsNotNull(_tools.SelectedItemClass);
        startPosition = Clamp(startPosition);
        DetectorItemViewModel item = new(_tools.SelectedItemClass, startPosition, _resizer, this);
        Items.Add(item);
        _drawingData = new DrawingData(startPosition, item);
    }

    public void UpdateDrawing(Point intermediatePosition)
    {
        Guard.IsNotNull(_drawingData);
        intermediatePosition = Clamp(intermediatePosition);
        _drawingData.UpdateBounding(intermediatePosition);
    }

    public async void EndDrawing(Point finishPosition)
    {
        Guard.IsNotNull(_drawingData);
        var screenshot = _screenshots.SelectedScreenshot;
        Guard.IsNotNull(screenshot);
        Guard.IsNotNull(_tools.SelectedItemClass);
        finishPosition = Clamp(finishPosition);
        _drawingData.UpdateBounding(finishPosition);
        var boundingViewModel = _drawingData.ItemViewModel.Bounding;
        if (boundingViewModel.Width < MinimumDimensionSize || boundingViewModel.Height < MinimumDimensionSize)
        {
            Items.Remove(_drawingData.ItemViewModel);
            _drawingData = null;
            return;
        }
        boundingViewModel.Synchronize();
        _drawingData.ItemViewModel.Item = await _annotator.Annotate(screenshot.Item, _tools.SelectedItemClass, boundingViewModel.Bounding);
        screenshot.NotifyIsAssetChanged();
        _drawingData = null;
    }

    private static Point Clamp(Point point) => new(Math.Clamp(point.X, 0, 1), Math.Clamp(point.Y, 0, 1));

    public void Dispose() => _disposable.Dispose();

    private readonly AnnotatorScreenshotsViewModel _screenshots;
    private readonly AnnotatorToolsViewModel _tools;
    private readonly DetectorAnnotator _annotator;
    private readonly DetectorItemResizer _resizer;
    private readonly IDisposable _disposable;
    private DrawingData? _drawingData;
    [ObservableProperty] private bool _isItemSelectionEnabled;
    [ObservableProperty] private DetectorItemViewModel? _selectedItem;
    [ObservableProperty] private Size? _imageSize;

    partial void OnSelectedItemChanged(DetectorItemViewModel? value)
    {
        _tools.SelectedItem = value;
    }

    partial void OnIsItemSelectionEnabledChanged(bool value)
    {
        foreach (var item in Items)
            item.IsThumbsVisible = value;
    }
}