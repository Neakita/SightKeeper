using System.Threading.Tasks;
using FluentValidation;
using SightKeeper.Application.ImageSets;
using SightKeeper.Application.ImageSets.Creating;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.ImageSets.Dialogs;
using SightKeeper.Avalonia.Misc;

namespace SightKeeper.Avalonia.ImageSets.Commands;

public sealed class CreateImageSetCommand(
	IValidator<ImageSetData> validator,
	DialogManager dialogManager,
	ImageSetCreator imageSetCreator)
	: AsyncCommand
{
	protected override async Task Execute()
	{
		using ImageSetCreationDialogViewModel creationDialog = new(validator);
		if (await dialogManager.ShowDialogAsync(creationDialog))
			imageSetCreator.Create(creationDialog);
	}
}