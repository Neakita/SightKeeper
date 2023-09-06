using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using SightKeeper.Application.Annotating;
using SightKeeper.Commons;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed partial class AnnotatorToolsViewModel : ViewModel, IDisposable
{
    public IReadOnlyCollection<ItemClass> ItemClasses
    {
        get => _itemClasses;
        private set => SetProperty(ref _itemClasses, value);
    }

    public AnnotatorToolsViewModel(
        DetectorAnnotator annotator,
        SelectedDataSetViewModel selectedDataSetViewModel,
        SelectedScreenshotViewModel selectedScreenshotViewModel,
        AnnotatorScreenshotsViewModel screenshotsViewModel)
    {
        _annotator = annotator;
        _selectedScreenshotViewModel = selectedScreenshotViewModel;
        _screenshotsViewModel = screenshotsViewModel;
        selectedScreenshotViewModel.ObservableValue
            .Subscribe(OnScreenshotSelected)
            .DisposeWith(_disposable);
        selectedDataSetViewModel.ObservableValue
            .Subscribe(newValue => ItemClasses = newValue?.ItemClasses ?? Array.Empty<ItemClass>())
            .DisposeWithEx(_disposable);
        DetectorItemViewModel.ItemClassChanged.Subscribe(item =>
        {
            Guard.IsNotNull(item.Item);
            annotator.ChangeItemClass(item.Item, item.ItemClass);
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
        SelectedItemClassIndex = SelectedItemClassIndex.Cycle(0, ItemClasses.Count - 1, reverse);
    }

    public void Dispose()
    {
        _disposable.Dispose();
        _selectedScreenshotDisposable?.Dispose();
    }
    
    
    private readonly DetectorAnnotator _annotator;
    private readonly SelectedScreenshotViewModel _selectedScreenshotViewModel;
    private readonly AnnotatorScreenshotsViewModel _screenshotsViewModel;
    private readonly CompositeDisposable _disposable = new();

    private IDisposable? _selectedScreenshotDisposable;

    
    [ObservableProperty] private ItemClass? _selectedItemClass;
    [ObservableProperty] private int _selectedItemClassIndex;
    
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(DeleteItemCommand))]
    private DetectorItemViewModel? _selectedItem;

    private IReadOnlyCollection<ItemClass> _itemClasses = Array.Empty<ItemClass>();


    [RelayCommand(CanExecute = nameof(CanMarkSelectedScreenshotAsAsset))]
    private void MarkSelectedScreenshotAsAsset()
    {
        var screenshot = _selectedScreenshotViewModel.Value;
        Guard.IsNotNull(screenshot);
        _annotator.MarkAsset(screenshot.Item);
        screenshot.NotifyIsAssetChanged();
    }
    private bool CanMarkSelectedScreenshotAsAsset() =>
        _selectedScreenshotViewModel.Value?.IsAsset == false;

    [RelayCommand(CanExecute = nameof(CanUnMarkSelectedScreenshotAsAsset))]
    private void UnMarkSelectedScreenshotAsAsset()
    {
        var screenshot = _selectedScreenshotViewModel.Value;
        Guard.IsNotNull(screenshot);
        _annotator.UnMarkAsset(screenshot.Item);
        screenshot.NotifyIsAssetChanged();
        _selectedScreenshotViewModel.Items.Clear();
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
        _annotator.DeleteScreenshot(screenshot.Item);
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
        _annotator.DeleteItem(item.Item);
        _selectedScreenshotViewModel.Items.Remove(item);
    }

    private bool CanDeleteItem() => SelectedItem != null;

    private void OnScreenshotSelected(ScreenshotViewModel? screenshot)
    {
        _selectedScreenshotDisposable?.Dispose();
        if (screenshot == null)
            return;
        _selectedScreenshotDisposable = screenshot.WhenAnyValue(x => x.IsAsset).Subscribe(_ =>
        {
            MarkSelectedScreenshotAsAssetCommand.NotifyCanExecuteChanged();
            UnMarkSelectedScreenshotAsAssetCommand.NotifyCanExecuteChanged();
        });
    }
}