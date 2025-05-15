using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using Material.Icons;
using SightKeeper.Application.Annotation;
using SightKeeper.Application.Extensions;
using SightKeeper.Application.ScreenCapturing;
using SightKeeper.Avalonia.Annotation.Images;

namespace SightKeeper.Avalonia.Annotation.Tooling.Commands;

public sealed class DeleteSelectedImageButtonDefinitionFactory : AnnotationButtonDefinitionFactory
{
	public DeleteSelectedImageButtonDefinitionFactory(
		ImageSelection images,
		IReadOnlyCollection<ObservableAnnotator> annotators,
		ImageRepository imageRepository)
	{
		_images = images;
		_annotators = annotators;
		_imageRepository = imageRepository;
	}

	public AnnotationButtonDefinition CreateButtonDefinition() => new()
	{
		IconKind = MaterialIconKind.ImageRemove,
		Command = CreateCommand(),
		ToolTip = "Delete selected image"
	};

	private readonly ImageSelection _images;
	private readonly IReadOnlyCollection<ObservableAnnotator> _annotators;
	private readonly ImageRepository _imageRepository;

	private DisposableCommand CreateCommand()
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

	private bool CanDeleteImage => _images.SelectedImage?.Assets.Count == 0;

	private void DeleteImage()
	{
		Guard.IsNotNull(_images.Set);
		_imageRepository.DeleteImage(_images.Set, _images.SelectedImageIndex);
	}
}