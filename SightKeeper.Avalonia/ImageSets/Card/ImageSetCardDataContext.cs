using System.Windows.Input;

namespace SightKeeper.Avalonia.ImageSets.Card;

public interface ImageSetCardDataContext
{
	string Name { get; }
	bool IsCapturing { get; }
	ImageDataContext? PreviewImage { get; }
	ICommand EditCommand { get; }
	ICommand DeleteCommand { get; }
	ICommand StartCapturingCommand { get; }
	ICommand StopCapturingCommand { get; }
}