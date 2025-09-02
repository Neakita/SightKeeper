using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Avalonia.DataSets.Dialogs.Tags.Plain;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags.Poser;

public interface EditablePoserTagDataContext : EditableTagDataContext
{
	IReadOnlyCollection<EditableTagDataContext> KeyPointTags { get; }
	ICommand CreateKeyPointTagCommand { get; }
}