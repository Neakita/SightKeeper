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
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Avalonia.ScreenshotsLibraries;

internal sealed partial class ScreenshotsLibrariesViewModel : ViewModel, IScreenshotsLibrariesViewModel
{
	public IReadOnlyCollection<ScreenshotsLibraryViewModel> ScreenshotsLibraries { get; }

	public ScreenshotsLibrariesViewModel(
		ScreenshotsLibraryViewModelsObservableRepository screenshotsLibrariesObservableRepository,
		DialogManager dialogManager,
		ReadDataAccess<ScreenshotsLibrary> readScreenshotsLibrariesDataAccess,
		ScreenshotsLibraryCreator screenshotsLibraryCreator,
		ScreenshotsLibraryEditor screenshotsLibraryEditor,
		[Tag("new")] IValidator<ScreenshotsLibraryData> newScreenshotsLibraryDataValidator,
		IValidator<ScreenshotsLibraryData> screenshotsLibraryDataValidator,
		ScreenshotsLibrariesDeleter screenshotsLibrariesDeleter)
	{
		_dialogManager = dialogManager;
		_readScreenshotsLibrariesDataAccess = readScreenshotsLibrariesDataAccess;
		_screenshotsLibraryCreator = screenshotsLibraryCreator;
		_screenshotsLibraryEditor = screenshotsLibraryEditor;
		_newScreenshotsLibraryDataValidator = newScreenshotsLibraryDataValidator;
		_screenshotsLibraryDataValidator = screenshotsLibraryDataValidator;
		_screenshotsLibrariesDeleter = screenshotsLibrariesDeleter;
		ScreenshotsLibraries = screenshotsLibrariesObservableRepository.Items;
	}

	private readonly DialogManager _dialogManager;
	private readonly ReadDataAccess<ScreenshotsLibrary> _readScreenshotsLibrariesDataAccess;
	private readonly ScreenshotsLibraryCreator _screenshotsLibraryCreator;
	private readonly ScreenshotsLibraryEditor _screenshotsLibraryEditor;
	private readonly IValidator<ScreenshotsLibraryData> _newScreenshotsLibraryDataValidator;
	private readonly IValidator<ScreenshotsLibraryData> _screenshotsLibraryDataValidator;
	private readonly ScreenshotsLibrariesDeleter _screenshotsLibrariesDeleter;

	[RelayCommand]
	private async Task CreateScreenshotsLibraryAsync()
	{
		using ScreenshotsLibraryDialogViewModel dialog = new("Create screenshots library",
			_newScreenshotsLibraryDataValidator);
		if (await _dialogManager.ShowDialogAsync(dialog))
			_screenshotsLibraryCreator.Create(dialog);
	}

	[RelayCommand]
	private async Task EditScreenshotsLibraryAsync(ScreenshotsLibrary screenshotsLibrary)
	{
		var header = $"Edit screenshots library '{screenshotsLibrary.Name}'";
		IValidator<ScreenshotsLibraryData> validator = new ExistingScreenshotsLibraryDataValidator(screenshotsLibrary,
			_screenshotsLibraryDataValidator, _readScreenshotsLibrariesDataAccess);
		using ScreenshotsLibraryDialogViewModel dialog = new(header, validator, screenshotsLibrary.Name,
			screenshotsLibrary.Description);
		if (await _dialogManager.ShowDialogAsync(dialog))
			_screenshotsLibraryEditor.EditLibrary(screenshotsLibrary, dialog);
	}

	[RelayCommand]
	private async Task DeleteScreenshotsLibraryAsync(ScreenshotsLibrary screenshotsLibrary)
	{
		if (!_screenshotsLibrariesDeleter.CanDelete(screenshotsLibrary))
		{
			var message =
				$"The library '{screenshotsLibrary.Name}' cannot be deleted as some dataset references it as asset. " +
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
			$"Are you sure you want to permanently delete the screenshots library '{screenshotsLibrary.Name}'? You will not be able to recover it.",
			deleteButton, cancelButton,
			new MessageBoxButtonDefinition("Cancel", MaterialIconKind.Cancel));
		if (await _dialogManager.ShowDialogAsync(deletionConfirmationDialog) == deleteButton)
			_screenshotsLibrariesDeleter.Delete(screenshotsLibrary);
	}

	ICommand IScreenshotsLibrariesViewModel.CreateScreenshotsLibraryCommand => CreateScreenshotsLibraryCommand;
	ICommand IScreenshotsLibrariesViewModel.EditScreenshotsLibraryCommand => EditScreenshotsLibraryCommand;
	ICommand IScreenshotsLibrariesViewModel.DeleteScreenshotsLibraryCommand => DeleteScreenshotsLibraryCommand;
}