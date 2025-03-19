using System.Windows.Input;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.ImageSets.Card;

internal sealed class ImageSetCardViewModel : ViewModel, ImageSetCardDataContext
{
	public ImageSet ImageSet { get; }
	public string Name => ImageSet.Name;
	public Image? PreviewImage => ImageSet.Images.RandomOrDefault();
	public ICommand EditCommand { get; }
	public ICommand DeleteCommand { get; }

	public ImageSetCardViewModel(ImageSet value, ICommand editCommand, ICommand deleteCommand)
	{
		ImageSet = value;
		EditCommand = editCommand;
		DeleteCommand = deleteCommand;
	}

	internal void NotifyPropertiesChanged()
	{
		OnPropertyChanged(nameof(Name));
	}
}