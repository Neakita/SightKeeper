using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CommunityToolkit.Diagnostics;
using DynamicData;
using SightKeeper.Application;
using SightKeeper.Application.Extensions;
using SightKeeper.Avalonia.Annotation.Assets;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Avalonia.Annotation;

internal sealed class ScreenshotsViewModel<TAssetViewModel, TAsset> : ScreenshotsViewModel
	where TAssetViewModel : AssetViewModel<TAsset>
	where TAsset : Asset, AssetsFactory<TAsset>, AssetsDestroyer<TAsset>
{
	public override ScreenshotsLibrary<TAsset> Library { get; }
	public override IReadOnlyCollection<ScreenshotViewModel<TAssetViewModel>> Screenshots { get; }

	public override IReadOnlyCollection<DateOnly> Dates => _dates;

	public ScreenshotsViewModel(
		ScreenshotsLibrary<TAsset> library,
		ObservableDataAccess<Screenshot> observableDataAccess,
		ScreenshotImageLoader imageLoader)
	{
		Library = library;
		_screenshotsSource.Connect()
			.Transform(screenshot => new ScreenshotViewModel<TAssetViewModel>(screenshot, imageLoader))
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
		Screenshots = screenshots;
		_dates = dates;
		observableDataAccess.Added
			.Where(screenshot => screenshot.Library == Library)
			.Cast<Screenshot<TAsset>>()
			.Subscribe(_screenshotsSource.Add)
			.DisposeWith(_disposable);
		observableDataAccess.Removed
			.Where(screenshot => screenshot.Library == Library)
			.Cast<Screenshot<TAsset>>()
			.Subscribe(screenshot => Guard.IsTrue(_screenshotsSource.Remove(screenshot)))
			.DisposeWith(_disposable);
	}

	private readonly CompositeDisposable _disposable = new();
	private readonly SourceList<Screenshot<TAsset>> _screenshotsSource = new();
	private readonly ReadOnlyObservableCollection<DateOnly> _dates;
}