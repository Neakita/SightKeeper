using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;
using DynamicData.Aggregation;
using DynamicData.Binding;
using SightKeeper.Application.Annotating;
using SightKeeper.Commons;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed partial class AnnotatorScreenshotsViewModel : ViewModel
{
    public SelectedScreenshotViewModel SelectedScreenshotViewModel { get; }
    public IReadOnlyList<ScreenshotViewModel> Screenshots { get; }
    public IObservable<int> TotalScreenshotsCount { get; }
    public IObservable<int> ScreenshotsWithoutAssetsCount { get; }
    public IObservable<int> ScreenshotsWithAssetsCount { get; }

    public IEnumerable<SortingRule<Screenshot>> SortingRules { get; } = new[]
    {
        new SortingRule<Screenshot>("New first", SortDirection.Descending, screenshot => screenshot.CreationDate),
        new SortingRule<Screenshot>("Old first", SortDirection.Ascending, screenshot => screenshot.CreationDate)
    };

    public bool IsLoading
    {
        get => _isLoading;
        private set => SetProperty(ref _isLoading, value);
    }

    [ObservableProperty] private SortingRule<Screenshot> _sortingRule;

    public AnnotatorScreenshotsViewModel(ScreenshotImageLoader imageLoader, ScreenshotsDataAccess screenshotsDataAccess, SelectedScreenshotViewModel selectedScreenshotViewModel)
    {
        SelectedScreenshotViewModel = selectedScreenshotViewModel;
        _screenshotsDataAccess = screenshotsDataAccess;
        _sortingRule = SortingRules.First();
        var sortingRule = this.WhenAnyValue(viewModel => viewModel.SortingRule)
            .Select(rule => rule.Comparer);
        _screenshots.Connect()
            .Sort(sortingRule)
            .Transform(screenshot => new ScreenshotViewModel(imageLoader, screenshot))
            .Bind(out var screenshots)
            .PopulateInto(_screenshotViewModels);
        TotalScreenshotsCount = _screenshots.Connect().Count();
        Screenshots = screenshots;
        ScreenshotsWithAssetsCount = _screenshotViewModels.Connect()
            .FilterOnObservable(screenshot => screenshot.IsAssetObservable)
            .Count();
        ScreenshotsWithoutAssetsCount = _screenshotViewModels.Connect()
            .FilterOnObservable(screenshot => screenshot.IsAssetObservable.Select(isAsset => !isAsset))
            .Count();
    }

    public void ScrollScreenshot(bool reverse)
    {
        if (Screenshots.Count <= 1 || SelectedScreenshotViewModel.SelectedScreenshotIndex == null)
            return;
        SelectedScreenshotViewModel.SelectedScreenshotIndex = SelectedScreenshotViewModel.SelectedScreenshotIndex.Value.Cycle(0, Screenshots.Count - 1, reverse);
    }

    private readonly ScreenshotsDataAccess _screenshotsDataAccess;
    private readonly SourceList<Screenshot> _screenshots = new();
    private readonly SourceList<ScreenshotViewModel> _screenshotViewModels = new();

    [ObservableProperty, NotifyPropertyChangedFor(nameof(TotalScreenshotsCount))]
    private DataSet? _dataSet;

    private CompositeDisposable? _dataSetDisposable;
    private bool _isLoading;
    private int? _screenshotsWithAssetsCount;
    private int? _screenshotsWithoutAssetsCount;

    partial void OnDataSetChanged(DataSet? value)
    {
        _dataSetDisposable?.Dispose();
        _screenshots.Clear();
        if (value == null)
            return;
        LoadScreenshots(value);
        _dataSetDisposable = new CompositeDisposable();
        value.ScreenshotsLibrary.ScreenshotAdded
            .Subscribe(OnScreenshotAdded)
            .DisposeWith(_dataSetDisposable);
        value.ScreenshotsLibrary.ScreenshotRemoved
            .Subscribe(OnScreenshotRemoved)
            .DisposeWith(_dataSetDisposable);
        OnPropertyChanged(nameof(TotalScreenshotsCount));
    }

    private async void LoadScreenshots(DataSet dataSet)
    {
        IsLoading = true;
        _screenshotsDataAccess.Load(
            dataSet.ScreenshotsLibrary,
            SortingRule.Direction == SortDirection.Descending)
            .Subscribe(screenshotsPartition => _screenshots.AddRange(screenshotsPartition), () => IsLoading = false);
    }

    private void OnScreenshotAdded(Screenshot newScreenshot) => _screenshots.Add(newScreenshot);
    private void OnScreenshotRemoved(Screenshot removedScreenshot) => _screenshots.Remove(removedScreenshot);
}