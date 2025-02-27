using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application.Annotation;
using SightKeeper.Application.Extensions;
using SightKeeper.Application.ScreenCapturing;
using SightKeeper.Avalonia.Annotation.Images;

namespace SightKeeper.Avalonia.Annotation.Tooling.Commands;

public sealed class DeleteSelectedImageCommandFactory
{
	public DeleteSelectedImageCommandFactory(
		ImageSelection images,
		IReadOnlyCollection<ObservableAnnotator> annotators,
		ImageDataAccess imageDataAccess)
	{
		_images = images;
		_annotators = annotators;
		_imageDataAccess = imageDataAccess;
	}

	public DisposableCommand CreateCommand()
	{
		RelayCommand command = new(DeleteImage, () => CanDeleteImage);
		CompositeDisposable disposable = new(2);
		_images.SelectedImageChanged
			.Subscribe(_ => command.NotifyCanExecuteChanged())
			.DisposeWith(disposable);
		_annotators
			.Select(annotator => annotator.AssetsChanged)
			.Merge()
			.Where(image => image == _images.SelectedImage)
			.Subscribe(_ => command.NotifyCanExecuteChanged())
			.DisposeWith(disposable);
		return new DisposableCommand(command, disposable);
	}

	private readonly ImageSelection _images;
	private readonly IReadOnlyCollection<ObservableAnnotator> _annotators;
	private readonly ImageDataAccess _imageDataAccess;

	private bool CanDeleteImage => _images.SelectedImage?.Assets.Count == 0;

	private void DeleteImage()
	{
		Guard.IsNotNull(_images.Set);
		_imageDataAccess.DeleteImage(_images.Set, _images.SelectedImageIndex);
	}
}