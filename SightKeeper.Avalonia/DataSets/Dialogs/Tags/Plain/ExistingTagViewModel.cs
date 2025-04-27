using Avalonia.Media;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags.Plain;

internal sealed class ExistingTagViewModel : EditableTagViewModel, EditedTagData
{
	public Tag Tag { get; }

	uint EditedTagData.Color => Color.ToUInt32();

	public bool IsEffectivelyEdited => Name != Tag.Name || Color.ToUInt32() != Tag.Color;

	public ExistingTagViewModel(Tag tag)
	{
		Tag = tag;
		Name = tag.Name;
		Color = Color.FromUInt32(tag.Color);
	}
}