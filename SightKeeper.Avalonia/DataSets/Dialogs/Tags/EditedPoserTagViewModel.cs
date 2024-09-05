using Avalonia.Media;
using FluentValidation;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags;

internal sealed class EditedPoserTagViewModel : PoserNewTagViewModel, EditedTagData
{
	Tag ExistingTagData.Tag => Tag;
	public PoserTag Tag { get; }

	public EditedPoserTagViewModel(IValidator<NewTagData> validator, PoserTag tag) : base(tag.Name, validator)
	{
		Tag = tag;
		Color = Color.FromUInt32(tag.Color);
	}
}