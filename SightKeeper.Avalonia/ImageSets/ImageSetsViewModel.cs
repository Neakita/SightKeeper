using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using Material.Icons;
using SightKeeper.Application;
using SightKeeper.Application.ImageSets;
using SightKeeper.Application.ImageSets.Creating;
using SightKeeper.Application.ImageSets.Editing;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.Dialogs.MessageBox;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.ImageSets;

internal sealed partial class ImageSetsViewModel : ViewModel, ImageSetsDataContext
{
	public IReadOnlyCollection<ImageSetViewModel> ImageSets { get; }

	public ImageSetsViewModel(
		ImageSetViewModelsObservableRepository imageSetsObservableRepository,
		DialogManager dialogManager,
		ReadDataAccess<ImageSet> readImageSetsDataAccess,
		ImageSetCreator imageSetCreator,
		ImageSetEditor imageSetEditor,
		[Tag("new")] IValidator<ImageSetData> newImageSetDataValidator,
		IValidator<ImageSetData> imageSetDataValidator,
		ImageSetDeleter imageSetDeleter)
	{
		_dialogManager = dialogManager;
		_readImageSetsDataAccess = readImageSetsDataAccess;
		_imageSetCreator = imageSetCreator;
		_imageSetEditor = imageSetEditor;
		_newImageSetDataValidator = newImageSetDataValidator;
		_imageSetDataValidator = imageSetDataValidator;
		_imageSetDeleter = imageSetDeleter;
		ImageSets = imageSetsObservableRepository.Items;
	}

	private readonly DialogManager _dialogManager;
	private readonly ReadDataAccess<ImageSet> _readImageSetsDataAccess;
	private readonly ImageSetCreator _imageSetCreator;
	private readonly ImageSetEditor _imageSetEditor;
	private readonly IValidator<ImageSetData> _newImageSetDataValidator;
	private readonly IValidator<ImageSetData> _imageSetDataValidator;
	private readonly ImageSetDeleter _imageSetDeleter;

	[RelayCommand]
	private async Task CreateImageSetAsync()
	{
		using ImageSetDialogViewModel dialog = new("Create image set",
			_newImageSetDataValidator);
		if (await _dialogManager.ShowDialogAsync(dialog))
			_imageSetCreator.Create(dialog);
	}

	[RelayCommand]
	private async Task EditImageSetAsync(ImageSet imageSet)
	{
		var header = $"Edit image set '{imageSet.Name}'";
		IValidator<ImageSetData> validator = new ExistingImageSetDataValidator(imageSet,
			_imageSetDataValidator, _readImageSetsDataAccess);
		using ImageSetDialogViewModel dialog = new(header, validator, imageSet.Name,
			imageSet.Description);
		if (await _dialogManager.ShowDialogAsync(dialog))
			_imageSetEditor.EditLibrary(imageSet, dialog);
	}

	[RelayCommand]
	private async Task DeleteImageSetAsync(ImageSet imageSet)
	{
		if (!_imageSetDeleter.CanDelete(imageSet))
		{
			var message =
				$"The library '{imageSet.Name}' cannot be deleted as some dataset references it as asset. " +
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
			$"Are you sure you want to permanently delete the image set '{imageSet.Name}'? You will not be able to recover it.",
			deleteButton, cancelButton,
			new MessageBoxButtonDefinition("Cancel", MaterialIconKind.Cancel));
		if (await _dialogManager.ShowDialogAsync(deletionConfirmationDialog) == deleteButton)
			_imageSetDeleter.Delete(imageSet);
	}

	ICommand ImageSetsDataContext.CreateImageSetCommand => CreateImageSetCommand;
	ICommand ImageSetsDataContext.EditImageSetCommand => EditImageSetCommand;
	ICommand ImageSetsDataContext.DeleteImageSetCommand => DeleteImageSetCommand;
}