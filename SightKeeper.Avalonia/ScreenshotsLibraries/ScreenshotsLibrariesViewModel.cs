using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using Material.Icons;
using SightKeeper.Application;
using SightKeeper.Application.ScreenshotsLibraries;
using SightKeeper.Application.ScreenshotsLibraries.Creating;
using SightKeeper.Application.ScreenshotsLibraries.Editing;
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
		ScreenshotsLibraryCreator screenshotsLibraryCreator,
		ScreenshotsLibraryEditor screenshotsLibraryEditor,
		[Tag("new")] IValidator<ScreenshotsLibraryData> newScreenshotsLibraryDataValidator,
		IValidator<ScreenshotsLibraryData> screenshotsLibraryDataValidator,
		ImageSetDeleter imageSetDeleter)
	{
		_dialogManager = dialogManager;
		_readScreenshotsLibrariesDataAccess = readScreenshotsLibrariesDataAccess;
		_screenshotsLibraryCreator = screenshotsLibraryCreator;
		_screenshotsLibraryEditor = screenshotsLibraryEditor;
		_newScreenshotsLibraryDataValidator = newScreenshotsLibraryDataValidator;
		_screenshotsLibraryDataValidator = screenshotsLibraryDataValidator;
		_imageSetDeleter = imageSetDeleter;
		ScreenshotsLibraries = screenshotsLibrariesObservableRepository.Items;
	}

	private readonly DialogManager _dialogManager;
	private readonly ReadDataAccess<ImageSet> _readScreenshotsLibrariesDataAccess;
	private readonly ScreenshotsLibraryCreator _screenshotsLibraryCreator;
	private readonly ScreenshotsLibraryEditor _screenshotsLibraryEditor;
	private readonly IValidator<ScreenshotsLibraryData> _newScreenshotsLibraryDataValidator;
	private readonly IValidator<ScreenshotsLibraryData> _screenshotsLibraryDataValidator;
	private readonly ImageSetDeleter _imageSetDeleter;

	[RelayCommand]
	private async Task CreateScreenshotsLibraryAsync()
	{
		using ScreenshotsLibraryDialogViewModel dialog = new("Create screenshots library",
			_newScreenshotsLibraryDataValidator);
		if (await _dialogManager.ShowDialogAsync(dialog))
			_screenshotsLibraryCreator.Create(dialog);
	}

	[RelayCommand]
	private async Task EditScreenshotsLibraryAsync(ImageSet imageSet)
	{
		var header = $"Edit screenshots library '{imageSet.Name}'";
		IValidator<ScreenshotsLibraryData> validator = new ExistingScreenshotsLibraryDataValidator(imageSet,
			_screenshotsLibraryDataValidator, _readScreenshotsLibrariesDataAccess);
		using ScreenshotsLibraryDialogViewModel dialog = new(header, validator, imageSet.Name,
			imageSet.Description);
		if (await _dialogManager.ShowDialogAsync(dialog))
			_screenshotsLibraryEditor.EditLibrary(imageSet, dialog);
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