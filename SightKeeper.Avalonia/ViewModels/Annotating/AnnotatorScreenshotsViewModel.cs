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
using Serilog;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

internal sealed partial class AnnotatorScreenshotsViewModel : ViewModel, IActivatableViewModel
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
                UpdateVisibleScreenshotsCategory();
        }
    }

    public bool ShowScreenshotsWithoutAssets
    {
        get => _showScreenshotsWithoutAssets;
        set
        {
            if (SetProperty(ref _showScreenshotsWithoutAssets, value))
                UpdateVisibleScreenshotsCategory();
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

    public AnnotatorScreenshotsViewModel(
	    ScreenshotsDataAccess screenshotsDataAccess,
	    SelectedScreenshotViewModel selectedScreenshotViewModel,
	    ObjectsLookupper objectsLookupper)
    {
        SelectedScreenshotViewModel = selectedScreenshotViewModel;
        _screenshotsDataAccess = screenshotsDataAccess;
        _objectsLookupper = objectsLookupper;
        _sortingRule = SortingRules.First();
        var sortingRule = this.WhenAnyValue(viewModel => viewModel.SortingRule)
            .Select(rule => rule.Comparer);
        _screenshotsSource.Connect()
            .Sort(sortingRule)
            .Transform(screenshot => new ScreenshotViewModel(screenshotsDataAccess, screenshot, _objectsLookupper))
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
        UpdateVisibleScreenshotsCategory();
        this.WhenActivated(OnActivated);
    }

    private void OnActivated(CompositeDisposable disposable)
    {
        _logger.Debug("Activated");
        Disposable.Create(OnDeactivated).DisposeWith(disposable);
        UpdateVisibleScreenshotsCategory();
    }
    
    private void OnDeactivated()
    {
        _logger.Debug("Deactivated");
        UpdateVisibleScreenshotsCategory(true);
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
    private readonly ObjectsLookupper _objectsLookupper;
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
    private readonly ILogger _logger = Log.ForContext<AnnotatorScreenshotsViewModel>();

    partial void OnDataSetChanged(DataSet? value)
    {
        _dataSetDisposable?.Dispose();
        _screenshotsSource.Clear();
        if (value == null)
            return;
        LoadScreenshots(value);
        _dataSetDisposable = new CompositeDisposable();
        _screenshotsDataAccess.ScreenshotAdded
	        .Where(screenshot => _objectsLookupper.GetLibrary(screenshot) == value.Screenshots)
            .Subscribe(OnScreenshotAdded)
            .DisposeWith(_dataSetDisposable);
        _screenshotsDataAccess.ScreenshotRemoved
	        .Where(screenshot => _objectsLookupper.GetLibrary(screenshot) == value.Screenshots)
            .Subscribe(OnScreenshotRemoved)
            .DisposeWith(_dataSetDisposable);
        OnPropertyChanged(nameof(TotalScreenshotsCount));
    }

    private void LoadScreenshots(DataSet dataSet)
    {
        IsLoading = true;
        _screenshotsSource.AddRange(dataSet.Screenshots);
        IsLoading = false;
    }

    private void OnScreenshotAdded(Screenshot newScreenshot) => _screenshotsSource.Add(newScreenshot);
    private void OnScreenshotRemoved(Screenshot removedScreenshot) => _screenshotsSource.Remove(removedScreenshot);

    [MemberNotNull(nameof(_screenshots))]
    private void UpdateVisibleScreenshotsCategory(bool hideAnyway = false)
    {
        _screenshots = null!;
        if (hideAnyway)
        {
            Screenshots = Array.Empty<ScreenshotViewModel>();
            return;
        }
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

    public ViewModelActivator Activator { get; } = new();
}