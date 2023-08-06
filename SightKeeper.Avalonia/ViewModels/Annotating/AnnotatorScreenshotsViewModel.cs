using System;
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

    public IEnumerable<SortingRule<Screenshot>> SortingRules { get; } = new[]
    {
        new SortingRule<Screenshot>("Old first", SortDirection.Ascending, screenshot => screenshot.CreationDate),
        new SortingRule<Screenshot>("New first", SortDirection.Descending, screenshot => screenshot.CreationDate)
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

    [ObservableProperty] private Model? _model;
    [ObservableProperty] private ScreenshotViewModel? _selectedScreenshot;
    [ObservableProperty] private int _selectedScreenshotIndex;
    
    private CompositeDisposable? _modelDisposable;

    partial void OnModelChanged(Model? value)
    {
        _modelDisposable?.Dispose();
        _screenshots.Clear();
        if (value == null)
            return;
        _screenshotsDataAccess.Load(value.ScreenshotsLibrary);
        _screenshots.AddRange(value.ScreenshotsLibrary.Screenshots);
        _modelDisposable = new CompositeDisposable();
        value.ScreenshotsLibrary.ScreenshotAdded
            .Subscribe(newScreenshot => _screenshots.Add(newScreenshot))
            .DisposeWith(_modelDisposable);
        value.ScreenshotsLibrary.ScreenshotRemoved
            .Subscribe(removedScreenshot => _screenshots.Remove(removedScreenshot))
            .DisposeWith(_modelDisposable);
    }

    partial void OnSelectedScreenshotChanged(ScreenshotViewModel? value) => _selectedScreenshotChanged.OnNext(value);
}