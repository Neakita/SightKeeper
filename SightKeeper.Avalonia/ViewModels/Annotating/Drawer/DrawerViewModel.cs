﻿using System;
using System.Linq;
using System.Reactive.Disposables;
using Avalonia;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Screenshots;

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
    
    public bool CanBeginDrawing => _tools.SelectedTag != null && SelectedScreenshotViewModel.Value != null;

    public DrawerViewModel(
        AnnotatorToolsViewModel tools,
        DetectorItemResizer resizer,
        SelectedScreenshotViewModel selectedScreenshotViewModel)
    {
	    throw new NotImplementedException();
	    /*SelectedScreenshotViewModel = selectedScreenshotViewModel;
	    _resizer = resizer;
	    _tools = tools;
	    selectedScreenshotViewModel.ObservableValue.Subscribe(_ =>
	    {
		    if (SelectedScreenshotViewModel.Value == null)
			    return;
		    var asset = SelectedScreenshotViewModel.Value.Item.Asset;
	        foreach (var item in asset.Items.Select(item => new DetectorItemViewModel(item, resizer, this)))
	            SelectedScreenshotViewModel.DetectorItems.Add(item);
	    }).DisposeWith(_disposable);

	    DetectedItemViewModel.MakeAnnotationRequested.Subscribe(item =>
	    {
	        Guard.IsNotNull(SelectedScreenshotViewModel.Value);
	        SelectedScreenshotViewModel.DetectedItems.Remove(item);
	        var screenshot = SelectedScreenshotViewModel.Value.Item;
	        var asset = GetOrMakeAsset(screenshot);
	        var detectorItem = asset.CreateItem(item.Tag, item.Bounding.Bounding);
	        DetectorItemViewModel detectorItemViewModel = new(detectorItem, resizer, this)
	        {
	            IsThumbsVisible = IsItemSelectionEnabled
	        };
	        SelectedScreenshotViewModel.DetectorItems.Add(detectorItemViewModel);
	        SelectedScreenshotViewModel.Value.NotifyIsAssetChanged();
	    }).DisposeWith(_disposable);*/
    }

    private DetectorAsset GetOrMakeAsset(Screenshot valueItem)
    {
	    throw new NotImplementedException();
	    // return _objectsLookupper.GetOptionalAsset(valueItem) ??
	    //        _objectsLookupper.GetDataSet(_objectsLookupper.GetLibrary(valueItem)).Assets.MakeAsset(valueItem);
    }

    public void BeginDrawing(Point startPosition)
    {
        Guard.IsNull(_drawingData);
        Guard.IsNotNull(_tools.SelectedTag);
        startPosition = Clamp(startPosition);
        DetectorItemViewModel item = new(_tools.SelectedTag, startPosition, _resizer, this);
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
	    throw new NotImplementedException();
	    // Guard.IsNotNull(_drawingData);
	    // var screenshot = SelectedScreenshotViewModel.Value;
	    // Guard.IsNotNull(screenshot);
	    // Guard.IsNotNull(_tools.SelectedTag);
	    // finishPosition = Clamp(finishPosition);
	    // _drawingData.UpdateBounding(finishPosition);
	    // var boundingViewModel = _drawingData.ItemViewModel.Bounding;
	    // if (boundingViewModel.Width < MinimumDimensionSize || boundingViewModel.Height < MinimumDimensionSize)
	    // {
	    //     SelectedScreenshotViewModel.DetectorItems.Remove(_drawingData.ItemViewModel);
	    //     _drawingData = null;
	    //     return;
	    // }
	    // boundingViewModel.Synchronize();
	    // var asset = GetOrMakeAsset(screenshot.Item);
	    // _drawingData.ItemViewModel.Item = asset.CreateItem(_tools.SelectedTag, boundingViewModel.Bounding);
	    // screenshot.NotifyIsAssetChanged();
	    // _drawingData = null;
    }

    private static Point Clamp(Point point) => new(Math.Clamp(point.X, 0, 1), Math.Clamp(point.Y, 0, 1));

    public void Dispose() => _disposable.Dispose();

    private readonly AnnotatorToolsViewModel _tools = null!;
    private readonly DetectorItemResizer _resizer = null!;
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