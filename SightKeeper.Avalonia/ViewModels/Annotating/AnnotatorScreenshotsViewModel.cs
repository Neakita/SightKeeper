﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using SightKeeper.Application.Annotating;
using SightKeeper.Commons;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed partial class AnnotatorScreenshotsViewModel : ViewModel
{
    private readonly ScreenshotsDataAccess _screenshotsDataAccess;
    public IObservable<ScreenshotViewModel?> SelectedScreenshotChanged => _selectedScreenshotChanged.AsObservable();
    public IReadOnlyList<ScreenshotViewModel> Screenshots { get; }
    public int? ScreenshotsCount => DataSet == null ? null : Screenshots.Count;

    public IEnumerable<SortingRule<Screenshot>> SortingRules { get; } = new[]
    {
        new SortingRule<Screenshot>("New first", SortDirection.Descending, screenshot => screenshot.CreationDate),
        new SortingRule<Screenshot>("Old first", SortDirection.Ascending, screenshot => screenshot.CreationDate)
    };

    [ObservableProperty] private SortingRule<Screenshot> _sortingRule;

    public AnnotatorScreenshotsViewModel(ScreenshotImageLoader imageLoader, ScreenshotsDataAccess screenshotsDataAccess)
    {
        _screenshotsDataAccess = screenshotsDataAccess;
        _sortingRule = SortingRules.First();
        var sortingRule = this.WhenAnyValue(viewModel => viewModel.SortingRule)
            .Select(rule => rule.Comparer);
        _screenshots.Connect()
            .Sort(sortingRule)
            .Transform(screenshot => new ScreenshotViewModel(imageLoader, screenshot))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out var screenshots)
            .Subscribe();
        Screenshots = screenshots;
    }

    public void ScrollScreenshot(bool reverse)
    {
        if (Screenshots.Count <= 1)
            return;
        SelectedScreenshotIndex = SelectedScreenshotIndex.Cycle(0, Screenshots.Count - 1, reverse);
    }

    private readonly SourceList<Screenshot> _screenshots = new();
    private readonly Subject<ScreenshotViewModel?> _selectedScreenshotChanged = new();

    [ObservableProperty, NotifyPropertyChangedFor(nameof(ScreenshotsCount))]
    private DataSet? _dataSet;
    [ObservableProperty] private ScreenshotViewModel? _selectedScreenshot;
    [ObservableProperty] private int _selectedScreenshotIndex;
    
    private CompositeDisposable? _dataSetDisposable;

    partial void OnDataSetChanged(DataSet? value)
    {
        _dataSetDisposable?.Dispose();
        _screenshots.Clear();
        if (value == null)
            return;
        _screenshotsDataAccess.Load(value.ScreenshotsLibrary);
        _screenshots.AddRange(value.ScreenshotsLibrary.Screenshots);
        _dataSetDisposable = new CompositeDisposable();
        value.ScreenshotsLibrary.ScreenshotAdded
            .Subscribe(OnScreenshotAdded)
            .DisposeWith(_dataSetDisposable);
        value.ScreenshotsLibrary.ScreenshotRemoved
            .Subscribe(OnScreenshotRemoved)
            .DisposeWith(_dataSetDisposable);
    }

    private void OnScreenshotAdded(Screenshot newScreenshot)
    {
        _screenshots.Add(newScreenshot);
        OnPropertyChanged(nameof(ScreenshotsCount));
    }

    private void OnScreenshotRemoved(Screenshot removedScreenshot)
    {
        _screenshots.Remove(removedScreenshot);
        OnPropertyChanged(nameof(ScreenshotsCount));
    }

    partial void OnSelectedScreenshotChanged(ScreenshotViewModel? value) => _selectedScreenshotChanged.OnNext(value);
}