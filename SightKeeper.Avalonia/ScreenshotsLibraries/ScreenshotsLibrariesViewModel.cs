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

namespace SightKeeper.Avalonia.ScreenshotsLibraries;

internal sealed partial class ScreenshotsLibrariesViewModel : ViewModel, ScreenshotsLibrariesDataContext
{
	public IReadOnlyCollection<ScreenshotsLibraryViewModel> ScreenshotsLibraries { get; }

	public ScreenshotsLibrariesViewModel(
		ScreenshotsLibraryViewModelsObservableRepository screenshotsLibrariesObservableRepository,
		DialogManager dialogManager,
		ReadDataAccess<ImageSet> readScreenshotsLibrariesDataAccess,
		ImageSetCreator imageSetCreator,
		ImageSetEditor imageSetEditor,
		[Tag("new")] IValidator<ImageSetData> newScreenshotsLibraryDataValidator,
		IValidator<ImageSetData> screenshotsLibraryDataValidator,
		ImageSetDeleter imageSetDeleter)
	{
		_dialogManager = dialogManager;
		_readScreenshotsLibrariesDataAccess = readScreenshotsLibrariesDataAccess;
		_imageSetCreator = imageSetCreator;
		_imageSetEditor = imageSetEditor;
		_newScreenshotsLibraryDataValidator = newScreenshotsLibraryDataValidator;
		_screenshotsLibraryDataValidator = screenshotsLibraryDataValidator;
		_imageSetDeleter = imageSetDeleter;
		ScreenshotsLibraries = screenshotsLibrariesObservableRepository.Items;
	}

	private readonly DialogManager _dialogManager;
	private readonly ReadDataAccess<ImageSet> _readScreenshotsLibrariesDataAccess;
	private readonly ImageSetCreator _imageSetCreator;
	private readonly ImageSetEditor _imageSetEditor;
	private readonly IValidator<ImageSetData> _newScreenshotsLibraryDataValidator;
	private readonly IValidator<ImageSetData> _screenshotsLibraryDataValidator;
	private readonly ImageSetDeleter _imageSetDeleter;

	[RelayCommand]
	private async Task CreateScreenshotsLibraryAsync()
	{
		using ImageSetDialogViewModel dialog = new("Create screenshots library",
			_newScreenshotsLibraryDataValidator);
		if (await _dialogManager.ShowDialogAsync(dialog))
			_imageSetCreator.Create(dialog);
	}

	[RelayCommand]
	private async Task EditScreenshotsLibraryAsync(ImageSet imageSet)
	{
		var header = $"Edit screenshots library '{imageSet.Name}'";
		IValidator<ImageSetData> validator = new ExistingImageSetDataValidator(imageSet,
			_screenshotsLibraryDataValidator, _readScreenshotsLibrariesDataAccess);
		using ImageSetDialogViewModel dialog = new(header, validator, imageSet.Name,
			imageSet.Description);
		if (await _dialogManager.ShowDialogAsync(dialog))
			_imageSetEditor.EditLibrary(imageSet, dialog);
	}

	[RelayCommand]
	private async Task DeleteScreenshotsLibraryAsync(ImageSet imageSet)
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
			"Screenshots library deletion confirmation",
			$"Are you sure you want to permanently delete the screenshots library '{imageSet.Name}'? You will not be able to recover it.",
			deleteButton, cancelButton,
			new MessageBoxButtonDefinition("Cancel", MaterialIconKind.Cancel));
		if (await _dialogManager.ShowDialogAsync(deletionConfirmationDialog) == deleteButton)
			_imageSetDeleter.Delete(imageSet);
	}

	ICommand ScreenshotsLibrariesDataContext.CreateScreenshotsLibraryCommand => CreateScreenshotsLibraryCommand;
	ICommand ScreenshotsLibrariesDataContext.EditScreenshotsLibraryCommand => EditScreenshotsLibraryCommand;
	ICommand ScreenshotsLibrariesDataContext.DeleteScreenshotsLibraryCommand => DeleteScreenshotsLibraryCommand;
}