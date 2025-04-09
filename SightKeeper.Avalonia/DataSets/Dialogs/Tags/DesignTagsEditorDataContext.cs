using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags;

internal sealed class DesignTagsEditorDataContext : TagsEditorDataContext
{
	public static DesignTagsEditorDataContext PlainTags => new()
	{
		Tags =
		[
			new DesignEditableTagDataContext("Head"),
			new DesignEditableTagDataContext("Body")
		]
	};

	public static DesignTagsEditorDataContext PoserTags => new()
	{
		Tags =
		[
			new DesignEditableTagDataContext("Enemy"),
			new DesignEditableTagDataContext("Ally")
		]
	};

	public IReadOnlyCollection<EditableTagDataContext> Tags { get; init; } =
		ReadOnlyCollection<EditableTagDataContext>.Empty;

	public ICommand CreateTagCommand => new RelayCommand(() => { });

	public DesignTagsEditorDataContext()
	{
	}

	public DesignTagsEditorDataContext(params IEnumerable<string> tagNames)
	{
		Tags = tagNames.Select(name => new DesignEditableTagDataContext(name)).ToList();
	}
}