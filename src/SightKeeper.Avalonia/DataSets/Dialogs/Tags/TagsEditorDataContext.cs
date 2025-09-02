using System.Windows.Input;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags;

public interface TagsEditorDataContext
{
	ICommand CreateTagCommand { get; }
}