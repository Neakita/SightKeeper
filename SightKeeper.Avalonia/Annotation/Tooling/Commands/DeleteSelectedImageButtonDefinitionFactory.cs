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
	public DeleteSelectedImageButtonDefinitionFactory(ImageSelection images)
	{
		_images = images;
	}

	public AnnotationButtonDefinition CreateButtonDefinition() => new()
	{
		IconKind = MaterialIconKind.ImageRemove,
		Command = CreateCommand(),
		ToolTip = "Delete selected image"
	};

	private readonly ImageSelection _images;

	private DisposableCommand CreateCommand()
	{
		RelayCommand command = new(DeleteImage, () => CanDeleteImage);
		CompositeDisposable disposable = new(2);
		_images.SelectedImageChanged
			.Subscribe(_ => command.NotifyCanExecuteChanged())
			.DisposeWith(disposable);
		return new DisposableCommand(command, disposable);
	}

	private bool CanDeleteImage => _images.SelectedImage?.Assets.Count == 0;

	private void DeleteImage()
	{
		Guard.IsNotNull(_images.Set);
		_images.Set.RemoveImageAt(_images.SelectedImageIndex);
	}
}