using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags;

internal sealed class DesignPlainTagsEditorDataContext : PlainTagsEditorDataContext
{
	public static DesignPlainTagsEditorDataContext Instance => new("Head", "Body");

	public ICommand CreateTagCommand => new RelayCommand(() => { });
	public IReadOnlyCollection<EditableTagDataContext> Tags { get; }

	public DesignPlainTagsEditorDataContext(params IEnumerable<string> tagNames)
	{
		Tags = tagNames.Select(name => new DesignEditableTagDataContext(name)).ToList();
	}
}