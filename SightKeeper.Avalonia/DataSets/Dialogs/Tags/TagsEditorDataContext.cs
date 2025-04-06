using System.Collections.Generic;
using System.Windows.Input;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags;

public interface TagsEditorDataContext
{
	IReadOnlyCollection<EditableTagDataContext> Tags { get; }
	ICommand CreateTagCommand { get; }
}