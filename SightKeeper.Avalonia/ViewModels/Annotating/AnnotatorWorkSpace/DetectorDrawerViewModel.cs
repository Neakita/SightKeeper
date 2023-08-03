using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using Avalonia;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed class DetectorDrawerViewModel : ViewModel, AnnotatorWorkSpace<DetectorModel>, IDisposable
{
    public ScreenshotViewModel? Screenshot => _screenshots.SelectedScreenshot;
    public DetectorAsset? Asset => (DetectorAsset?)Screenshot?.Item.Asset;
    public ObservableCollection<DetectorItemViewModel> Items { get; } = new();
    public bool CanBeginDrawing => _tools.SelectedItemClass != null && _screenshots.SelectedScreenshot != null;

    public DetectorDrawerViewModel(AnnotatorScreenshotsViewModel screenshots, DetectorAnnotatorToolsViewModel tools, DetectorAnnotator annotator)
    {
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
    }
    
    public void BeginDrawing(Point startPosition)
    {
        Guard.IsNull(_drawingData);
        Guard.IsNotNull(_tools.SelectedItemClass);
        DetectorItemViewModel item = new(_tools.SelectedItemClass, startPosition);
        Items.Add(item);
        _drawingData = new DrawingData(startPosition, item);
    }

    public void UpdateDrawing(Point intermediatePosition)
    {
        Guard.IsNotNull(_drawingData);
        _drawingData.UpdateBounding(intermediatePosition);
    }

    public void EndDrawing(Point finishPosition)
    {
        Guard.IsNotNull(_drawingData);
        var screenshot = _screenshots.SelectedScreenshot;
        Guard.IsNotNull(screenshot);
        Guard.IsNotNull(_tools.SelectedItemClass);
        _drawingData.UpdateBounding(finishPosition);
        var boundingViewModel = _drawingData.Item.Bounding;
        if (boundingViewModel.Width < 0.01 || boundingViewModel.Height < 0.01)
        {
            Items.Remove(_drawingData.Item);
            _drawingData = null;
            return;
        }
        boundingViewModel.Synchronize();
        _annotator.Annotate(screenshot.Item, _tools.SelectedItemClass, boundingViewModel.Bounding);
        screenshot.NotifyIsAssetChanged();
        _drawingData = null;
    }

    private readonly AnnotatorScreenshotsViewModel _screenshots;
    private readonly DetectorAnnotatorToolsViewModel _tools;
    private readonly DetectorAnnotator _annotator;
    private readonly IDisposable _disposable;
    private DrawingData? _drawingData;

    private sealed class DrawingData
    {
        public Point StartPosition { get; set; }
        public DetectorItemViewModel Item { get; }

        public DrawingData(Point startPosition, DetectorItemViewModel item)
        {
            StartPosition = startPosition;
            Item = item;
        }

        public void UpdateBounding(Point currentPosition) =>
            Item.Bounding.SetFromTwoPositions(StartPosition, currentPosition);
    }

    public void Dispose() => _disposable.Dispose();
}