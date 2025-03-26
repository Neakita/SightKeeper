using System.Windows.Input;

namespace SightKeeper.Avalonia.ImageSets.Card;

public interface ImageSetCardDataContext
{
	string Name { get; }
	ImageDataContext? PreviewImage { get; }
	ICommand EditCommand { get; }
	ICommand DeleteCommand { get; }
}