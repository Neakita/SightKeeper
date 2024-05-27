using System;
using System.Linq;
using System.Reactive.Disposables;
using Avalonia;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;

namespace SightKeeper.Avalonia.ViewModels.Annotating.Drawer;

internal sealed partial class DrawerViewModel : ViewModel, IDisposable
{
    public const double MinimumDimensionSize = 0.005;

    public SelectedScreenshotViewModel SelectedScreenshotViewModel { get; }

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
    
    public bool CanBeginDrawing => _tools.SelectedItemClass != null && SelectedScreenshotViewModel.Value != null;

    public DrawerViewModel(
        AnnotatorToolsViewModel tools,
        DetectorItemResizer resizer,
        SelectedScreenshotViewModel selectedScreenshotViewModel)
    {
        SelectedScreenshotViewModel = selectedScreenshotViewModel;
        _resizer = resizer;
        _tools = tools;
        selectedScreenshotViewModel.ObservableValue.Subscribe(_ =>
        {
            var asset = SelectedScreenshotViewModel.Value?.Item.Asset;
            if (asset == null)
                return;
            foreach (var item in asset.Items.Select(item => new DetectorItemViewModel(item, resizer, this)))
                SelectedScreenshotViewModel.DetectorItems.Add(item);
        }).DisposeWith(_disposable);

        DetectedItemViewModel.MakeAnnotationRequested.Subscribe(item =>
        {
            Guard.IsNotNull(SelectedScreenshotViewModel.Value);
            SelectedScreenshotViewModel.DetectedItems.Remove(item);
            var asset = SelectedScreenshotViewModel.Value.Item.Asset ??
                        SelectedScreenshotViewModel.Value.Item.MakeAsset();
            var detectorItem = asset.CreateItem(item.ItemClass, item.Bounding.Bounding);
            DetectorItemViewModel detectorItemViewModel = new(detectorItem, resizer, this)
            {
                IsThumbsVisible = IsItemSelectionEnabled
            };
            SelectedScreenshotViewModel.DetectorItems.Add(detectorItemViewModel);
            SelectedScreenshotViewModel.Value.NotifyIsAssetChanged();
        }).DisposeWith(_disposable);
    }
    
    public void BeginDrawing(Point startPosition)
    {
        Guard.IsNull(_drawingData);
        Guard.IsNotNull(_tools.SelectedItemClass);
        startPosition = Clamp(startPosition);
        DetectorItemViewModel item = new(_tools.SelectedItemClass, startPosition, _resizer, this);
        SelectedScreenshotViewModel.DetectorItems.Add(item);
        _drawingData = new DrawingData(startPosition, item);
    }

    public void UpdateDrawing(Point intermediatePosition)
    {
        Guard.IsNotNull(_drawingData);
        intermediatePosition = Clamp(intermediatePosition);
        _drawingData.UpdateBounding(intermediatePosition);
    }

    public void EndDrawing(Point finishPosition)
    {
        Guard.IsNotNull(_drawingData);
        var screenshot = SelectedScreenshotViewModel.Value;
        Guard.IsNotNull(screenshot);
        Guard.IsNotNull(_tools.SelectedItemClass);
        finishPosition = Clamp(finishPosition);
        _drawingData.UpdateBounding(finishPosition);
        var boundingViewModel = _drawingData.ItemViewModel.Bounding;
        if (boundingViewModel.Width < MinimumDimensionSize || boundingViewModel.Height < MinimumDimensionSize)
        {
            SelectedScreenshotViewModel.DetectorItems.Remove(_drawingData.ItemViewModel);
            _drawingData = null;
            return;
        }
        boundingViewModel.Synchronize();
        var asset = screenshot.Item.Asset ?? screenshot.Item.MakeAsset();
        _drawingData.ItemViewModel.Item = asset.CreateItem(_tools.SelectedItemClass, boundingViewModel.Bounding);
        screenshot.NotifyIsAssetChanged();
        _drawingData = null;
    }

    private static Point Clamp(Point point) => new(Math.Clamp(point.X, 0, 1), Math.Clamp(point.Y, 0, 1));

    public void Dispose() => _disposable.Dispose();

    private readonly AnnotatorToolsViewModel _tools;
    private readonly DetectorItemResizer _resizer;
    private readonly CompositeDisposable _disposable = new();
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
        foreach (var item in SelectedScreenshotViewModel.Items.OfType<DetectorItemViewModel>())
            item.IsThumbsVisible = value;
    }
}