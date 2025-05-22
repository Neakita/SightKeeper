using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Avalonia.Collections;
using Avalonia.Media;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Avalonia.DataSets.Dialogs.Tags.Plain;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags.Poser;

internal sealed partial class ExistingPoserTagViewModel : EditableTagViewModel, EditablePoserTagDataContext, EditedPoserTagData, TagsChanges
{
	public PoserTag Tag { get; }

	uint EditedTagData.Color => Color.ToUInt32();

	public bool IsEffectivelyEdited => Name != Tag.Name || Color.ToUInt32() != Tag.Color;

	public ExistingPoserTagViewModel(PoserTag tag)
	{
		Tag = tag;
		Name = tag.Name;
		Color = Color.FromUInt32(tag.Color);
		foreach (var keyPointTag in tag.KeyPointTags)
		{
			ExistingTagViewModel keyPointTagViewModel = new(keyPointTag);
			_keyPointTags.Add(keyPointTagViewModel);
		}
	}

	public IReadOnlyCollection<EditableTagDataContext> KeyPointTags => _keyPointTags;

	ICommand EditablePoserTagDataContext.CreateKeyPointTagCommand => CreateKeyPointTagCommand;

	public TagsChanges KeyPointTagsChanges => this;

	public IEnumerable<Tag> RemovedTags => Enumerable.Empty<Tag>();

	public IEnumerable<EditedTagData> EditedTags =>
		_keyPointTags.OfType<ExistingTagViewModel>().Where(tag => tag.IsEffectivelyEdited);

	public IEnumerable<NewTagData> NewTags => _keyPointTags.OfType<NewPoserTagViewModel>();

	private readonly AvaloniaList<EditableTagViewModel> _keyPointTags = new();

	[RelayCommand]
	private void CreateKeyPointTag(string name)
	{
		NewPoserTagViewModel tag = new()
		{
			Name = name
		};
		_keyPointTags.Add(tag);
	}
}