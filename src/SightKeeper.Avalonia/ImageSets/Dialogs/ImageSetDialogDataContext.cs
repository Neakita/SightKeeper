using System.Windows.Input;

namespace SightKeeper.Avalonia.ImageSets.Dialogs;

public interface ImageSetDialogDataContext
{
	string Name { get; set; }
	string Description { get; set; }
	ICommand ApplyCommand { get; }
	ICommand CancelCommand { get; }
}