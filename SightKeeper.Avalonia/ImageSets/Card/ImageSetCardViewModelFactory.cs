using System.Windows.Input;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Avalonia.ImageSets.Commands;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.ImageSets.Card;

internal sealed class ImageSetCardViewModelFactory
{
	public ImageSetCardViewModelFactory(
		ImageLoader imageLoader,
		EditImageSetCommandFactory editImageSetCommandFactory,
		DeleteImageSetCommandFactory deleteImageSetCommandFactory)
	{
		_imageLoader = imageLoader;
		_editImageSetCommand = editImageSetCommandFactory.CreateCommand();
		_deleteImageSetCommand = deleteImageSetCommandFactory.CreateCommand();
	}

	public ImageSetCardViewModel CreateImageSetCardViewModel(ImageSet imageSet)
	{
		ParametrizedCommandAdapter editCommand = new(_editImageSetCommand, imageSet);
		ParametrizedCommandAdapter deleteCommand = new(_deleteImageSetCommand, imageSet);
		return new ImageSetCardViewModel(imageSet, editCommand, deleteCommand, _imageLoader);
	}

	private readonly ICommand _editImageSetCommand;
	private readonly ICommand _deleteImageSetCommand;
	private readonly ImageLoader _imageLoader;
}