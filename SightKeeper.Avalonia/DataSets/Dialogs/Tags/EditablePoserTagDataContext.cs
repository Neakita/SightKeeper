using System.Collections.Generic;
using System.Windows.Input;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags;

public interface EditablePoserTagDataContext : EditableTagDataContext
{
	IReadOnlyCollection<EditableTagDataContext> KeyPointTags { get; }
	ICommand CreateKeyPointTagCommand { get; }
}