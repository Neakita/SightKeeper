using System.Threading.Tasks;
using FluentValidation;
using SightKeeper.Application.ImageSets.Editing;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.ImageSets.Dialogs;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.ImageSets.Commands;

public sealed class EditImageSetCommand(
	ImageSetEditor imageSetEditor,
	DialogManager dialogManager,
	IValidator<ExistingImageSetData> validator)
	: AsyncCommand<ImageSet>
{
	protected override async Task ExecuteAsync(ImageSet parameter)
	{
		using ImageSetEditingDialogViewModel dialog = new(parameter, validator);
		if (await dialogManager.ShowDialogAsync(dialog))
			imageSetEditor.EditImageSet(dialog);
	}
}