using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;
using SightKeeper.Application;
using SightKeeper.Application.Extensions;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Avalonia.Annotation;

internal sealed partial class ScreenshotsViewModel : ViewModel
{
	public ScreenshotsLibrary? Library
	{
		get => _library;
		set
		{
			if (!SetProperty(ref _library, value))
				return;
			_screenshotsSource.Clear();
			if (value == null)
				return;
			_screenshotsSource.AddRange(value.Screenshots);
		}
	}

	public IReadOnlyCollection<ScreenshotViewModel> DisplayScreenshots { get; }
	public IReadOnlyCollection<DateOnly> Dates { get; }

	public ScreenshotsViewModel(ObservableDataAccess<Screenshot> observableDataAccess, ScreenshotsDataAccess screenshotsDataAccess)
	{
		_screenshotsSource.Connect()
			.Transform(screenshot => new ScreenshotViewModel(screenshot, screenshotsDataAccess))
			.Bind(out var screenshots)
			.Subscribe()
			.DisposeWith(_disposable);
		_screenshotsSource.Connect()
			.Transform(screenshot => screenshot.CreationDate.Date)
			.Transform(DateOnly.FromDateTime)
			.DistinctValues(date => date)
			.Bind(out var dates)
			.Subscribe()
			.DisposeWith(_disposable);
		DisplayScreenshots = screenshots;
		Dates = dates;
		observableDataAccess.Added
			.Where(screenshot => screenshot.Library == Library)
			.Subscribe(_screenshotsSource.Add)
			.DisposeWith(_disposable);
		observableDataAccess.Removed
			.Where(screenshot => screenshot.Library == Library)
			.Subscribe(screenshot => Guard.IsTrue(_screenshotsSource.Remove(screenshot)))
			.DisposeWith(_disposable);
	}

	private readonly CompositeDisposable _disposable = new();
	private readonly SourceList<Screenshot> _screenshotsSource = new();
	private ScreenshotsLibrary? _library;
	[ObservableProperty] private ScreenshotViewModel? _selectedScreenshot;
}