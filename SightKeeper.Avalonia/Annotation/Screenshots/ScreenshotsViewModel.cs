using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;
using SightKeeper.Application;
using SightKeeper.Application.Annotation;
using SightKeeper.Application.Extensions;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Screenshots;

public sealed partial class ScreenshotsViewModel : ViewModel, ScreenshotSelection, AnnotationScreenshotsComponent
{
	public ImageSet? Library
	{
		get;
		set
		{
			if (!SetProperty(ref field, value))
				return;
			_screenshotsSource.Edit(source =>
			{
				source.Clear();
				if (value != null)
					source.AddRange(value.Screenshots);
			});
		}
	}

	public IReadOnlyCollection<ScreenshotViewModel> Screenshots { get; }
	[ObservableProperty] public partial int SelectedScreenshotIndex { get; set; } = -1;
	public Image? SelectedImage => SelectedScreenshotIndex >= 0 ? _screenshotsSource.Items[SelectedScreenshotIndex] : null;
	public ScreenshotViewModel? SelectedScreenshotViewModel => SelectedImage != null ? _screenshotsCache.Lookup(SelectedImage).Value : null;
	public IObservable<Unit> SelectedScreenshotChanged => _selectedScreenshotChanged.AsObservable();

	public ScreenshotsViewModel(
		ObservableScreenshotsDataAccess observableDataAccess,
		IEnumerable<ObservableAnnotator> observableAnnotators)
	{
		_screenshotsSource = new SourceList<Image>();
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
	private readonly SourceList<Image> _screenshotsSource;
	private readonly SourceCache<ScreenshotViewModel, Image> _screenshotsCache = new(viewModel => viewModel.Value);
	private readonly Subject<Unit> _selectedScreenshotChanged = new();

	private void OnScreenshotAssetsChanged(Image image)
	{
		var screenshotViewModel = _screenshotsCache.Lookup(image).Value;
		screenshotViewModel.NotifyAssetsChanged();
	}

	partial void OnSelectedScreenshotIndexChanged(int value)
	{
		_selectedScreenshotChanged.OnNext(Unit.Default);
	}
}