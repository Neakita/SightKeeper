using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using DynamicData.Aggregation;
using DynamicData.Binding;
using ReactiveUI;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed partial class AnnotatorScreenshotsViewModel : ViewModel
{
    public SelectedScreenshotViewModel SelectedScreenshotViewModel { get; }

    public IReadOnlyList<ScreenshotViewModel> Screenshots
    {
        get => _screenshots;
        private set => SetProperty(ref _screenshots, value);
    }

    public IObservable<int> TotalScreenshotsCount { get; }
    public IObservable<int> ScreenshotsWithoutAssetsCount { get; }
    public IObservable<int> ScreenshotsWithAssetsCount { get; }
    public bool ShowScreenshotsWithAssets
    {
        get => _showScreenshotsWithAssets;
        set
        {
            if (SetProperty(ref _showScreenshotsWithAssets, value))
                SetScreenshots();
        }
    }

    public bool ShowScreenshotsWithoutAssets
    {
        get => _showScreenshotsWithoutAssets;
        set
        {
            if (SetProperty(ref _showScreenshotsWithoutAssets, value))
                SetScreenshots();
        }
    }

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
        _screenshotsSource.Connect()
            .Sort(sortingRule)
            .Transform(screenshot => new ScreenshotViewModel(imageLoader, screenshot))
            .Bind(out var screenshots)
            .PopulateInto(_screenshotViewModels);
        TotalScreenshotsCount = _screenshotsSource.Connect().Count();
        _allScreenshots = screenshots;
        ScreenshotsWithAssetsCount = _screenshotViewModels.Connect()
            .FilterOnObservable(screenshot => screenshot.IsAssetObservable)
            .Count();
        ScreenshotsWithoutAssetsCount = _screenshotViewModels.Connect()
            .FilterOnObservable(screenshot => screenshot.IsAssetObservable.Select(isAsset => !isAsset))
            .Count();
        _screenshotViewModels.Connect()
            .AutoRefreshOnObservable(_ => _actualizeFilterSubject)
            .Filter(screenshot => screenshot.IsAsset)
            .Bind(out var screenshotsWithAssets)
            .Subscribe();
        _screenshotsWithAssetsAndSelected = screenshotsWithAssets;
        _screenshotViewModels.Connect()
            .AutoRefreshOnObservable(_ => _actualizeFilterSubject)
            .Filter(screenshot => !screenshot.IsAsset)
            .Bind(out var screenshotsWithoutAssets)
            .Subscribe();
        _screenshotsWithoutAssets = screenshotsWithoutAssets;
        SetScreenshots();
    }

    public void ScrollScreenshot(bool reverse)
    {
        if (Screenshots.Count <= 1 || SelectedScreenshotViewModel.SelectedScreenshotIndex == null)
            return;
        var indexDelta = reverse ? -1 : 1;
        var newIndex = SelectedScreenshotViewModel.SelectedScreenshotIndex.Value + indexDelta;
        SelectedScreenshotViewModel.SelectedScreenshotIndex = Math.Clamp(newIndex, 0, Screenshots.Count - 1);
    }

    private readonly ScreenshotsDataAccess _screenshotsDataAccess;
    private readonly SourceList<Screenshot> _screenshotsSource = new();
    private readonly ReadOnlyObservableCollection<ScreenshotViewModel> _allScreenshots;
    private readonly ReadOnlyObservableCollection<ScreenshotViewModel> _screenshotsWithAssetsAndSelected;
    private readonly ReadOnlyObservableCollection<ScreenshotViewModel> _screenshotsWithoutAssets;
    private readonly SourceList<ScreenshotViewModel> _screenshotViewModels = new();

    [ObservableProperty, NotifyPropertyChangedFor(nameof(TotalScreenshotsCount))]
    private DataSet? _dataSet;

    private CompositeDisposable? _dataSetDisposable;
    private bool _isLoading;
    private bool _showScreenshotsWithAssets = true;
    private bool _showScreenshotsWithoutAssets = true;
    private IReadOnlyList<ScreenshotViewModel> _screenshots;
    private readonly Subject<Unit> _actualizeFilterSubject = new();

    partial void OnDataSetChanged(DataSet? value)
    {
        _dataSetDisposable?.Dispose();
        _screenshotsSource.Clear();
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
        var screenshots = await _screenshotsDataAccess.LoadAll(dataSet.ScreenshotsLibrary);
        _screenshotsSource.AddRange(screenshots);
        IsLoading = false;
    }

    private void OnScreenshotAdded(Screenshot newScreenshot) => _screenshotsSource.Add(newScreenshot);
    private void OnScreenshotRemoved(Screenshot removedScreenshot) => _screenshotsSource.Remove(removedScreenshot);

    [MemberNotNull(nameof(_screenshots))]
    private void SetScreenshots()
    {
        _screenshots = null!;
        if (ShowScreenshotsWithAssets && ShowScreenshotsWithoutAssets)
            Screenshots = _allScreenshots;
        else if (ShowScreenshotsWithAssets)
            Screenshots = _screenshotsWithAssetsAndSelected;
        else if (ShowScreenshotsWithoutAssets)
            Screenshots = _screenshotsWithoutAssets;
        else
            Screenshots = Array.Empty<ScreenshotViewModel>();
    }

    [RelayCommand]
    private void ActualizeFiltering() => _actualizeFilterSubject.OnNext(Unit.Default);
}