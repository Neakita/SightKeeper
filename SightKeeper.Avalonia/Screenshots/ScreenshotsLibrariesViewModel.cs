using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
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

namespace SightKeeper.Avalonia.Screenshots;

internal sealed partial class ScreenshotsLibrariesViewModel : ViewModel, IScreenshotsLibrariesViewModel
{
	public IReadOnlyCollection<ScreenshotsLibraryViewModel> ScreenshotsLibraries { get; }

	public ScreenshotsLibrariesViewModel(
		ScreenshotsLibraryViewModelsObservableRepository screenshotsLibrariesObservableRepository,
		DialogManager dialogManager,
		ReadDataAccess<ScreenshotsLibrary> readScreenshotsLibrariesDataAccess,
		ScreenshotsLibraryCreator screenshotsLibraryCreator,
		ScreenshotsLibraryEditor screenshotsLibraryEditor,
		WriteDataAccess<ScreenshotsLibrary> writeScreenshotsLibrariesDataAccess,
		[Tag("new")] IValidator<ScreenshotsLibraryData> newScreenshotsLibraryDataValidator,
		IValidator<ScreenshotsLibraryData> screenshotsLibraryDataValidator)
	{
		_dialogManager = dialogManager;
		_readScreenshotsLibrariesDataAccess = readScreenshotsLibrariesDataAccess;
		_screenshotsLibraryCreator = screenshotsLibraryCreator;
		_screenshotsLibraryEditor = screenshotsLibraryEditor;
		_writeScreenshotsLibrariesDataAccess = writeScreenshotsLibrariesDataAccess;
		_newScreenshotsLibraryDataValidator = newScreenshotsLibraryDataValidator;
		_screenshotsLibraryDataValidator = screenshotsLibraryDataValidator;
		ScreenshotsLibraries = screenshotsLibrariesObservableRepository.Items;
	}

	private readonly DialogManager _dialogManager;
	private readonly ReadDataAccess<ScreenshotsLibrary> _readScreenshotsLibrariesDataAccess;
	private readonly ScreenshotsLibraryCreator _screenshotsLibraryCreator;
	private readonly ScreenshotsLibraryEditor _screenshotsLibraryEditor;
	private readonly WriteDataAccess<ScreenshotsLibrary> _writeScreenshotsLibrariesDataAccess;
	private readonly IValidator<ScreenshotsLibraryData> _newScreenshotsLibraryDataValidator;
	private readonly IValidator<ScreenshotsLibraryData> _screenshotsLibraryDataValidator;

	[ObservableProperty] private ScreenshotsLibraryViewModel? _selectedScreenshotsLibrary;

	[RelayCommand]
	private async Task CreateScreenshotsLibraryAsync()
	{
		using ScreenshotsLibraryDialogViewModel dialog = new("Create screenshots library", _newScreenshotsLibraryDataValidator);
		if (await _dialogManager.ShowDialogAsync(dialog))
			_screenshotsLibraryCreator.Create(dialog);
	}

	[RelayCommand]
	private async Task EditScreenshotsLibraryAsync(ScreenshotsLibrary screenshotsLibrary)
	{
		var header = $"Edit screenshots library '{screenshotsLibrary.Name}'";
		IValidator<ScreenshotsLibraryData> validator = new ExistingScreenshotsLibraryDataValidator(screenshotsLibrary, _screenshotsLibraryDataValidator, _readScreenshotsLibrariesDataAccess);
		using ScreenshotsLibraryDialogViewModel dialog = new(header, validator, screenshotsLibrary.Name, screenshotsLibrary.Description);
		if (await _dialogManager.ShowDialogAsync(dialog))
			_screenshotsLibraryEditor.EditLibrary(screenshotsLibrary, dialog);
	}

	[RelayCommand]
	private async Task DeleteScreenshotsLibraryAsync(ScreenshotsLibrary screenshotsLibrary)
	{
		MessageBoxButtonDefinition deletionButton = new("Delete", MaterialIconKind.Delete);
		MessageBoxDialogViewModel dialog = new(
			"Screenshots library deletion confirmation",
			$"Are you sure you want to permanently delete the screenshots library '{screenshotsLibrary.Name}'? You will not be able to recover it.",
			deletionButton,
			new MessageBoxButtonDefinition("Cancel", MaterialIconKind.Cancel));
		if (await _dialogManager.ShowDialogAsync(dialog) == deletionButton)
			_writeScreenshotsLibrariesDataAccess.Remove(screenshotsLibrary);
	}

	ICommand IScreenshotsLibrariesViewModel.CreateScreenshotsLibraryCommand => CreateScreenshotsLibraryCommand;
	ICommand IScreenshotsLibrariesViewModel.EditScreenshotsLibraryCommand => EditScreenshotsLibraryCommand;
	ICommand IScreenshotsLibrariesViewModel.DeleteScreenshotsLibraryCommand => DeleteScreenshotsLibraryCommand;
}