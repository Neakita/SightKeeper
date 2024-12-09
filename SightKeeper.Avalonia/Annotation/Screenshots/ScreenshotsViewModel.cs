using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;
using SightKeeper.Application;
using SightKeeper.Application.Extensions;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Avalonia.Annotation.Screenshots;

internal sealed partial class ScreenshotsViewModel : ViewModel
{
	public ScreenshotsLibrary? Library
	{
		get;
		set
		{
			if (!SetProperty(ref field, value))
				return;
			_screenshotsSource.Clear();
			if (value != null)
				_screenshotsSource.AddRange(value.Screenshots);
		}
	}

	public IReadOnlyCollection<ScreenshotViewModel> Screenshots { get; }
	public ScreenshotImageLoader ImageLoader { get; }

	public ScreenshotsViewModel(
		ObservableScreenshotsDataAccess observableDataAccess,
		ScreenshotImageLoader imageLoader)
	{
		ImageLoader = imageLoader;
		_screenshotsSource.Connect()
			.Transform(screenshot => new ScreenshotViewModel(screenshot))
			.Bind(out var screenshots)
			.Subscribe()
			.DisposeWith(_disposable);
		Screenshots = screenshots;
		observableDataAccess.Added
			.Where(data => data.library == Library)
			.Select(data => data.screenshot)
			.Subscribe(_screenshotsSource.Add)
			.DisposeWith(_disposable);
		observableDataAccess.Removed
			.Where(data => data.library == Library)
			.Select(data => data.screenshot)
			.Select(_screenshotsSource.Remove)
			.Subscribe(isRemoved => Guard.IsTrue(isRemoved))
			.DisposeWith(_disposable);
	}

	[ObservableProperty] private ScreenshotViewModel? _selectedScreenshot;

	private readonly CompositeDisposable _disposable = new();
	private readonly SourceList<Screenshot> _screenshotsSource = new();
}