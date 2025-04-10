using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags;

internal sealed class DesignPoserTagsEditorDataContext : PoserTagsEditorDataContext
{
	public static DesignPoserTagsEditorDataContext Instance => new()
	{
		Tags =
		[
			new DesignEditablePoserTagDataContext("Enemy", "Head", "Body"),
			new DesignEditablePoserTagDataContext("Ally")
		]
	};

	public ICommand CreateTagCommand => new RelayCommand(() => { });
	public IReadOnlyCollection<EditablePoserTagDataContext> Tags { get; init; } = ReadOnlyCollection<EditablePoserTagDataContext>.Empty;
}