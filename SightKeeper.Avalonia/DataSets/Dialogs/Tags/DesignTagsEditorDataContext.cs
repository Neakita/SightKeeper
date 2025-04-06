using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Media;
using CommunityToolkit.Mvvm.Input;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags;

internal sealed class DesignTagsEditorDataContext : TagsEditorDataContext
{
	public static DesignTagsEditorDataContext PlainTags => new()
	{
		Tags =
		[
			new DesignEditableTagDataContext("Head", Colors.IndianRed),
			new DesignEditableTagDataContext("Body", Colors.SeaGreen)
		]
	};

	public IReadOnlyCollection<EditableTagDataContext> Tags { get; init; } =
		ReadOnlyCollection<EditableTagDataContext>.Empty;

	public ICommand CreateTagCommand => new RelayCommand(() => { });
}