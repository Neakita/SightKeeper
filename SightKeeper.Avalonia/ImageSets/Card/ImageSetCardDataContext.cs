using System.Windows.Input;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.ImageSets.Card;

public interface ImageSetCardDataContext
{
	string Name { get; }
	Image? PreviewImage { get; }
	ICommand EditCommand { get; }
	ICommand DeleteCommand { get; }
}