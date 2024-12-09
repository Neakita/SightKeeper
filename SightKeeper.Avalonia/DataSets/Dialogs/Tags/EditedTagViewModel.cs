using Avalonia.Media;
using FluentValidation;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags;

internal sealed class EditedTagViewModel : TagDataViewModel, EditedTagData
{
	public Tag Tag { get; }

	public EditedTagViewModel(IValidator<NewTagData> validator, Tag tag) : base(tag.Name, validator)
	{
		Tag = tag;
		Color = Color.FromUInt32(tag.Color);
	}
}