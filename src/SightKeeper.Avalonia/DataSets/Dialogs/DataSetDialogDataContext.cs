using System.Windows.Input;
using SightKeeper.Avalonia.DataSets.Dialogs.Tags;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

public interface DataSetDialogDataContext
{
	DataSetEditorDataContext DataSetEditor { get; }
	TagsEditorDataContext TagsEditor { get; }
	DataSetTypePickerViewModel? TypePicker { get; }
	ICommand ApplyCommand { get; }
	ICommand CloseCommand { get; }
}