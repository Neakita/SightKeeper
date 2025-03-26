using System.Windows.Input;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.ImageSets.Card;

internal sealed class ImageSetCardViewModel : ViewModel, ImageSetCardDataContext
{
	public ImageSet ImageSet { get; }
	public string Name => ImageSet.Name;
	public ImageDataContext? PreviewImage { get; }
	public ICommand EditCommand { get; }
	public ICommand DeleteCommand { get; }

	public ImageSetCardViewModel(ImageSet value, ICommand editCommand, ICommand deleteCommand, ImageLoader imageLoader)
	{
		ImageSet = value;
		EditCommand = editCommand;
		DeleteCommand = deleteCommand;
		var image = ImageSet.Images.RandomOrDefault();
		if (image != null)
			PreviewImage = new ImageViewModel(imageLoader, image);
	}

	internal void NotifyPropertiesChanged()
	{
		OnPropertyChanged(nameof(Name));
	}
}