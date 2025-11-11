using System.Threading.Tasks;
using Material.Icons;
using SightKeeper.Application.Extensions;
using SightKeeper.Application.Misc;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.Dialogs.MessageBox;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.ImageSets.Commands;

public sealed class DeleteImageSetCommand(DialogManager dialogManager, WriteRepository<ImageSet> imageSetsRepository) : AsyncCommand<ImageSet>
{
	protected override async Task ExecuteAsync(ImageSet parameter)
	{
		if (!parameter.CanDelete())
		{
			await NotifyUserImageSetCannotBeDeleted(parameter);
			return;
		}
		if (await PromptUserForSetDeletionConfirmation(parameter))
			imageSetsRepository.Remove(parameter);
	}

	private async Task NotifyUserImageSetCannotBeDeleted(ImageSet parameter)
	{
		var message =
			$"The image set '{parameter.Name}' cannot be deleted as some dataset references it as asset. " +
			$"Delete all associated assets to be able delete the library.";
		MessageBoxButtonDefinition closeButton = new("Close", MaterialIconKind.Close, true);
		MessageBoxDialogViewModel dialog = new("The image set cannot be deleted", message, closeButton);
		await dialogManager.ShowDialogAsync(dialog);
	}

	private async Task<bool> PromptUserForSetDeletionConfirmation(ImageSet set)
	{
		MessageBoxButtonDefinition deleteButton = new("Delete", MaterialIconKind.Delete);
		MessageBoxButtonDefinition cancelButton = new("Cancel", MaterialIconKind.Close, true);
		MessageBoxDialogViewModel deletionConfirmationDialog = new(
			"Image set deletion confirmation",
			$"Are you sure you want to permanently delete the image set '{set.Name}'? You will not be able to recover it.",
			deleteButton, cancelButton);
		var pressedButton = await dialogManager.ShowDialogAsync(deletionConfirmationDialog);
		return pressedButton == deleteButton;
	}
}