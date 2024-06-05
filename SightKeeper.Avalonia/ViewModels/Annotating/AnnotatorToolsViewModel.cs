﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

internal sealed partial class AnnotatorToolsViewModel : ViewModel, IDisposable
{
    public IReadOnlyCollection<ItemClass> ItemClasses
    {
        get => _itemClasses;
        private set => SetProperty(ref _itemClasses, value);
    }

    public AnnotatorToolsViewModel(
        SelectedDataSetViewModel selectedDataSetViewModel,
        SelectedScreenshotViewModel selectedScreenshotViewModel,
        AnnotatorScreenshotsViewModel screenshotsViewModel,
        ObjectsLookupper objectsLookupper)
    {
        _selectedScreenshotViewModel = selectedScreenshotViewModel;
        _screenshotsViewModel = screenshotsViewModel;
        _objectsLookupper = objectsLookupper;
        selectedScreenshotViewModel.ObservableValue
            .Subscribe(OnScreenshotSelected)
            .DisposeWith(_disposable);
        selectedDataSetViewModel.ObservableValue
            .Subscribe(newValue => ItemClasses = newValue?.ItemClasses ?? Array.Empty<ItemClass>())
            .DisposeWith(_disposable);
        Drawer.DetectorItemViewModel.ItemClassChanged.Subscribe(item =>
        {
            Guard.IsNotNull(item.Item);
            item.Item.ItemClass = item.ItemClass;
        }).DisposeWith(_disposable);
        _selectedScreenshotViewModel.NotifyCanExecuteChanged(
            MarkSelectedScreenshotAsAssetCommand,
            UnMarkSelectedScreenshotAsAssetCommand,
            DeleteScreenshotCommand);
    }

    public void ScrollItemClass(bool reverse)
    {
        if (ItemClasses.Count <= 1)
            return;
        if (reverse)
	        SelectedItemClassIndex--;
        else
	        SelectedItemClassIndex++;
    }

    public void Dispose()
    {
        _disposable.Dispose();
        _selectedScreenshotDisposable?.Dispose();
    }
    
    
    private readonly SelectedScreenshotViewModel _selectedScreenshotViewModel;
    private readonly AnnotatorScreenshotsViewModel _screenshotsViewModel;
    private readonly ObjectsLookupper _objectsLookupper;
    private readonly CompositeDisposable _disposable = new();

    private IDisposable? _selectedScreenshotDisposable;

    
    [ObservableProperty] private ItemClass? _selectedItemClass;
    [ObservableProperty] private int _selectedItemClassIndex;
    
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(DeleteItemCommand))]
    private Drawer.DetectorItemViewModel? _selectedItem;

    private IReadOnlyCollection<ItemClass> _itemClasses = Array.Empty<ItemClass>();


    [RelayCommand(CanExecute = nameof(CanMarkSelectedScreenshotAsAsset))]
    private void MarkSelectedScreenshotAsAsset()
    {
        var screenshot = _selectedScreenshotViewModel.Value;
        Guard.IsNotNull(screenshot);
        _objectsLookupper.GetDataSet(_objectsLookupper.GetLibrary(screenshot.Item)).Assets.MakeAsset(screenshot.Item);
        screenshot.NotifyIsAssetChanged();
    }
    private bool CanMarkSelectedScreenshotAsAsset() =>
        _selectedScreenshotViewModel.Value?.IsAsset == false;

    [RelayCommand(CanExecute = nameof(CanUnMarkSelectedScreenshotAsAsset))]
    private void UnMarkSelectedScreenshotAsAsset()
    {
        var screenshot = _selectedScreenshotViewModel.Value;
        Guard.IsNotNull(screenshot);
        _objectsLookupper.GetDataSet(_objectsLookupper.GetLibrary(screenshot.Item)).Assets.DeleteAsset(_objectsLookupper.GetAsset(screenshot.Item));
        screenshot.NotifyIsAssetChanged();
        _selectedScreenshotViewModel.DetectorItems.Clear();
    }
    private bool CanUnMarkSelectedScreenshotAsAsset() =>
        _selectedScreenshotViewModel.Value?.IsAsset == true;

    [RelayCommand(CanExecute = nameof(CanDeleteScreenshot))]
    private void DeleteScreenshot()
    {
        var screenshot = _selectedScreenshotViewModel.Value;
        Guard.IsNotNull(_selectedScreenshotViewModel.SelectedScreenshotIndex);
        var screenshotIndex = _selectedScreenshotViewModel.SelectedScreenshotIndex.Value;
        Guard.IsNotNull(screenshot);
        _objectsLookupper.GetLibrary(screenshot.Item).DeleteScreenshot(screenshot.Item);
        var screenshots = _screenshotsViewModel.Screenshots;
        if (!screenshots.Any())
            return;
        _selectedScreenshotViewModel.SelectedScreenshotIndex = Math.Min(screenshotIndex, screenshots.Count - 1);
    }
    private bool CanDeleteScreenshot() => _selectedScreenshotViewModel.Value != null;

    [RelayCommand(CanExecute = nameof(CanDeleteItem))]
    private void DeleteItem()
    {
        var item = SelectedItem;
        Guard.IsNotNull(item);
        Guard.IsNotNull(item.Item);
        _objectsLookupper.GetAsset(item.Item).DeleteItem(item.Item);
        _selectedScreenshotViewModel.DetectorItems.Remove(item);
    }

    private bool CanDeleteItem() => SelectedItem != null;

    private void OnScreenshotSelected(ScreenshotViewModel? screenshot)
    {
        _selectedScreenshotDisposable?.Dispose();
        if (screenshot == null)
            return;
        _selectedScreenshotDisposable = screenshot.IsAssetObservable.Subscribe(_ =>
        {
            MarkSelectedScreenshotAsAssetCommand.NotifyCanExecuteChanged();
            UnMarkSelectedScreenshotAsAssetCommand.NotifyCanExecuteChanged();
        });
    }
}