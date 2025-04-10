using System.Collections.Generic;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags;

public interface PlainTagsEditorDataContext : TagsEditorDataContext
{
	IReadOnlyCollection<EditableTagDataContext> Tags { get; }
}