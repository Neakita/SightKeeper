using System.Windows.Input;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Avalonia.ImageSets.Commands;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.ImageSets.Card;

internal sealed class ImageSetCardViewModelFactory
{
	public ImageSetCardViewModelFactory(
		EditImageSetCommandFactory editImageSetCommandFactory,
		DeleteImageSetCommandFactory deleteImageSetCommandFactory,
		ImageLoader imageLoader)
	{
		_editImageSetCommand = editImageSetCommandFactory.CreateCommand();
		_deleteImageSetCommand = deleteImageSetCommandFactory.CreateCommand();
		_imageLoader = imageLoader;
	}

	public ImageSetCardViewModel CreateImageSetCardViewModel(ImageSet imageSet)
	{
		var editCommand = _editImageSetCommand.WithParameter(imageSet);
		var deleteCommand = _deleteImageSetCommand.WithParameter(imageSet);
		return new ImageSetCardViewModel(imageSet, editCommand, deleteCommand, _imageLoader);
	}

	private readonly ICommand _editImageSetCommand;
	private readonly ICommand _deleteImageSetCommand;
	private readonly ImageLoader _imageLoader;
}