using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Material.Icons;
using SightKeeper.Application.ImageSets;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.Dialogs.MessageBox;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.ImageSets.Commands;

internal sealed class DeleteImageSetCommandFactory
{

	public DeleteImageSetCommandFactory(
		ImageSetDeleter imageSetDeleter,
		DialogManager dialogManager)
	{
		_imageSetDeleter = imageSetDeleter;
		_dialogManager = dialogManager;
	}

	public ICommand CreateCommand()
	{
		return new AsyncRelayCommand<ImageSet>(DeleteImageSetAsync!);
	}

	private readonly ImageSetDeleter _imageSetDeleter;
	private readonly DialogManager _dialogManager;

	private async Task DeleteImageSetAsync(ImageSet set)
	{
		if (!ImageSetDeleter.CanDelete(set))
		{
			var message =
				$"The library '{set.Name}' cannot be deleted as some dataset references it as asset. " +
				$"Delete all associated assets to be able delete the library.";
			MessageBoxButtonDefinition closeButton = new("Close", MaterialIconKind.Close, true);
			MessageBoxDialogViewModel dialog = new("The library cannot be deleted", message, closeButton);
			await _dialogManager.ShowDialogAsync(dialog);
			return;
		}
		MessageBoxButtonDefinition deleteButton = new("Delete", MaterialIconKind.Delete);
		MessageBoxButtonDefinition cancelButton = new("Cancel", MaterialIconKind.Close, true);
		MessageBoxDialogViewModel deletionConfirmationDialog = new(
			"Image set deletion confirmation",
			$"Are you sure you want to permanently delete the image set '{set.Name}'? You will not be able to recover it.",
			deleteButton, cancelButton,
			new MessageBoxButtonDefinition("Cancel", MaterialIconKind.Cancel));
		if (await _dialogManager.ShowDialogAsync(deletionConfirmationDialog) == deleteButton)
			_imageSetDeleter.Delete(set);
	}
}