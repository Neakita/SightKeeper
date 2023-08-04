using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using Avalonia;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed partial class DetectorDrawerViewModel : ViewModel, AnnotatorWorkSpace<DetectorModel>, IDisposable
{
    public const double MinimumDimensionSize = 0.01;

    private sealed class DrawingData
    {
        public DetectorItemViewModel Item { get; }

        public DrawingData(Point startPosition, DetectorItemViewModel item)
        {
            _startPosition = startPosition;
            Item = item;
        }

        public void UpdateBounding(Point currentPosition) =>
            Item.Bounding.SetFromTwoPositions(_startPosition, currentPosition);
        
        private readonly Point _startPosition;
    }
    
    public ScreenshotViewModel? Screenshot => _screenshots.SelectedScreenshot;
    public DetectorAsset? Asset => (DetectorAsset?)Screenshot?.Item.Asset;
    public ObservableCollection<DetectorItemViewModel> Items { get; } = new();
    public bool CanBeginDrawing => _tools.SelectedItemClass != null && _screenshots.SelectedScreenshot != null;
    public DrawerItemResizer Resizer { get; }

    public DetectorDrawerViewModel(AnnotatorScreenshotsViewModel screenshots, DetectorAnnotatorToolsViewModel tools, DetectorAnnotator annotator, DrawerItemResizer resizer)
    {
        Resizer = resizer;
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
            if (asset == null) return;
            foreach (var item in asset.Items.Select(item => new DetectorItemViewModel(item)))
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
        DetectorItemViewModel item = new(_tools.SelectedItemClass, startPosition);
        Items.Add(item);
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
        var screenshot = _screenshots.SelectedScreenshot;
        Guard.IsNotNull(screenshot);
        Guard.IsNotNull(_tools.SelectedItemClass);
        finishPosition = Clamp(finishPosition);
        _drawingData.UpdateBounding(finishPosition);
        var boundingViewModel = _drawingData.Item.Bounding;
        if (boundingViewModel.Width < MinimumDimensionSize || boundingViewModel.Height < MinimumDimensionSize)
        {
            Items.Remove(_drawingData.Item);
            _drawingData = null;
            return;
        }
        boundingViewModel.Synchronize();
        _drawingData.Item.Item =  _annotator.Annotate(screenshot.Item, _tools.SelectedItemClass, boundingViewModel.Bounding);
        screenshot.NotifyIsAssetChanged();
        _drawingData = null;
    }

    private static Point Clamp(Point point) => new(Math.Clamp(point.X, 0, 1), Math.Clamp(point.Y, 0, 1));

    public void Dispose() => _disposable.Dispose();

    private readonly AnnotatorScreenshotsViewModel _screenshots;
    private readonly DetectorAnnotatorToolsViewModel _tools;
    private readonly DetectorAnnotator _annotator;
    private readonly IDisposable _disposable;
    private DrawingData? _drawingData;
    [ObservableProperty] private bool _isItemSelectionEnabled;
    [ObservableProperty] private DetectorItemViewModel? _selectedItem;

    partial void OnSelectedItemChanged(DetectorItemViewModel? value)
    {
        _tools.SelectedItem = value;
    }
}