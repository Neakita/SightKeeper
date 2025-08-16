using System.Windows.Input;

namespace SightKeeper.Avalonia.DataSets.Card;

public interface DataSetCardDataContext
{
	string Name { get; }
	ImageDataContext? Image { get; }
	ICommand EditCommand { get; }
	ICommand ExportCommand { get; }
	ICommand DeleteCommand { get; }
}