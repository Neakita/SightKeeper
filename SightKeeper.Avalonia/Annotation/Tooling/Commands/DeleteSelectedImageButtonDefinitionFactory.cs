using System;
using System.Reactive.Disposables;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using Material.Icons;
using SightKeeper.Application.Extensions;
using SightKeeper.Avalonia.Annotation.Images;

namespace SightKeeper.Avalonia.Annotation.Tooling.Commands;

public sealed class DeleteSelectedImageButtonDefinitionFactory : AnnotationButtonDefinitionFactory
{
	public DeleteSelectedImageButtonDefinitionFactory(ImageSelection imageSelection, ImageSetSelection imageSetSelection)
	{
		_imageSelection = imageSelection;
		_imageSetSelection = imageSetSelection;
	}

	public AnnotationButtonDefinition CreateButtonDefinition() => new()
	{
		IconKind = MaterialIconKind.ImageRemove,
		Command = CreateCommand(),
		ToolTip = "Delete selected image"
	};

	private readonly ImageSelection _imageSelection;
	private readonly ImageSetSelection _imageSetSelection;

	private DisposableCommand CreateCommand()
	{
		RelayCommand command = new(DeleteImage, () => CanDeleteImage);
		CompositeDisposable disposable = new(2);
		_imageSelection.SelectedImageChanged
			.Subscribe(_ => command.NotifyCanExecuteChanged())
			.DisposeWith(disposable);
		return new DisposableCommand(command, disposable);
	}

	private bool CanDeleteImage => _imageSelection.SelectedImage?.Assets.Count == 0;

	private void DeleteImage()
	{
		var set = _imageSetSelection.SelectedImageSet;
		Guard.IsNotNull(set);
		set.RemoveImageAt(_imageSelection.SelectedImageIndex);
	}
}