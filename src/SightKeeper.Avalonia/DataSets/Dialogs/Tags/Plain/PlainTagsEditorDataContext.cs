using System.Collections.Generic;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags.Plain;

public interface PlainTagsEditorDataContext : TagsEditorDataContext
{
	IReadOnlyCollection<EditableTagDataContext> Tags { get; }
}