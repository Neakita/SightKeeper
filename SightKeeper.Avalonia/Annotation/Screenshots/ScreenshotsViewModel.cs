using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;
using SightKeeper.Application;
using SightKeeper.Application.Extensions;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Avalonia.Annotation.Screenshots;

public sealed partial class ScreenshotsViewModel : ViewModel
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
	[ObservableProperty] public partial ScreenshotViewModel? SelectedScreenshot { get; set; }
	public IObservable<ScreenshotViewModel?> SelectedScreenshotChanged => _selectedScreenshotChanged.AsObservable();

	public ScreenshotsViewModel(
		ObservableScreenshotsDataAccess observableDataAccess,
		ScreenshotImageLoader imageLoader,
		IEnumerable<ObservableAnnotator> observableAnnotators)
	{
		ImageLoader = imageLoader;
		_screenshotsSource.Connect()
			.Transform(screenshot => new ScreenshotViewModel(screenshot))
			.AddKey(viewModel => viewModel.Value)
			.Bind(out var screenshots)
			.PopulateInto(_screenshotsCache)
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
		observableAnnotators
			.Select(annotator => annotator.AssetsChanged)
			.Merge()
			.Subscribe(OnScreenshotAssetsChanged)
			.DisposeWith(_disposable);
	}

	private readonly CompositeDisposable _disposable = new();
	private readonly SourceList<Screenshot> _screenshotsSource = new();
	private readonly SourceCache<ScreenshotViewModel, Screenshot> _screenshotsCache = new(viewModel => viewModel.Value);
	private readonly Subject<ScreenshotViewModel?> _selectedScreenshotChanged = new();

	private void OnScreenshotAssetsChanged(Screenshot screenshot)
	{
		var screenshotViewModel = _screenshotsCache.Lookup(screenshot).Value;
		screenshotViewModel.NotifyAssetsChanged();
	}

	partial void OnSelectedScreenshotChanged(ScreenshotViewModel? value)
	{
		_selectedScreenshotChanged.OnNext(value);
	}
}