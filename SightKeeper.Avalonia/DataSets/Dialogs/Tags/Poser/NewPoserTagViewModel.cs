using System.Collections.Generic;
using System.Windows.Input;
using Avalonia.Collections;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Avalonia.DataSets.Dialogs.Tags.Plain;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags.Poser;

internal partial class NewPoserTagViewModel : EditableTagViewModel, EditablePoserTagDataContext, NewPoserTagData
{
	uint NewTagData.Color => Color.ToUInt32();

	IReadOnlyCollection<EditableTagDataContext> EditablePoserTagDataContext.KeyPointTags => _keyPointTags;

	ICommand EditablePoserTagDataContext.CreateKeyPointTagCommand => CreateKeyPointTagCommand;

	IReadOnlyCollection<NewTagData> NewPoserTagData.KeyPointTags => _keyPointTags;

	[RelayCommand]
	private void CreateKeyPointTag(string name)
	{
		NewTagViewModel tag = new()
		{
			Name = name
		};
		_keyPointTags.Add(tag);
	}

	private readonly AvaloniaList<NewTagViewModel> _keyPointTags = new();
}