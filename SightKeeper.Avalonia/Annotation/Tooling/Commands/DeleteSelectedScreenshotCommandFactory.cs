using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application.Annotation;
using SightKeeper.Application.Extensions;
using SightKeeper.Application.Screenshotting;
using SightKeeper.Avalonia.Annotation.Screenshots;

namespace SightKeeper.Avalonia.Annotation.Tooling.Commands;

public sealed class DeleteSelectedScreenshotCommandFactory
{
	public DeleteSelectedScreenshotCommandFactory(
		ScreenshotSelection screenshots,
		IReadOnlyCollection<ObservableAnnotator> annotators,
		ImageDataAccess imageDataAccess)
	{
		_screenshots = screenshots;
		_annotators = annotators;
		_imageDataAccess = imageDataAccess;
	}

	public DisposableCommand CreateCommand()
	{
		RelayCommand command = new(DeleteScreenshot, () => CanDeleteScreenshot);
		CompositeDisposable disposable = new(2);
		_screenshots.SelectedScreenshotChanged
			.Subscribe(_ => command.NotifyCanExecuteChanged())
			.DisposeWith(disposable);
		_annotators
			.Select(annotator => annotator.AssetsChanged)
			.Merge()
			.Where(screenshot => screenshot == _screenshots.SelectedImage)
			.Subscribe(_ => command.NotifyCanExecuteChanged())
			.DisposeWith(disposable);
		return new DisposableCommand(command, disposable);
	}

	private readonly ScreenshotSelection _screenshots;
	private readonly IReadOnlyCollection<ObservableAnnotator> _annotators;
	private readonly ImageDataAccess _imageDataAccess;

	private bool CanDeleteScreenshot => _screenshots.SelectedImage?.Assets.Count == 0;

	private void DeleteScreenshot()
	{
		Guard.IsNotNull(_screenshots.Library);
		_imageDataAccess.DeleteImage(_screenshots.Library, _screenshots.SelectedScreenshotIndex);
	}
}