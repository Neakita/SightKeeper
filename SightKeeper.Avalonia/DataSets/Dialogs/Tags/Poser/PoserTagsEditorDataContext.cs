using System.Collections.Generic;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags.Poser;

public interface PoserTagsEditorDataContext : TagsEditorDataContext
{
	IReadOnlyCollection<EditablePoserTagDataContext> Tags { get; }
}