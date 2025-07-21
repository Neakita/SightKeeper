using System.Windows.Input;
using SightKeeper.Application.ScreenCapturing;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Avalonia.ImageSets.Commands;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.ImageSets.Card;

internal sealed class ImageSetCardViewModelFactory
{
	public ImageSetCardViewModelFactory(
		EditImageSetCommand editImageSetCommand,
		DeleteImageSetCommand deleteImageSetCommand,
		ImageLoader imageLoader,
		ImageCapturer capturer)
	{
		_editImageSetCommand = editImageSetCommand;
		_deleteImageSetCommand = deleteImageSetCommand;
		_imageLoader = imageLoader;
		_capturer = capturer;
	}

	public ImageSetCardViewModel CreateImageSetCardViewModel(ImageSet imageSet)
	{
		var editCommand = _editImageSetCommand.WithParameter(imageSet);
		var deleteCommand = _deleteImageSetCommand.WithParameter(imageSet);
		return new ImageSetCardViewModel(imageSet, editCommand, deleteCommand, _imageLoader, _capturer);
	}

	private readonly ICommand _editImageSetCommand;
	private readonly ICommand _deleteImageSetCommand;
	private readonly ImageLoader _imageLoader;
	private readonly ImageCapturer _capturer;
}