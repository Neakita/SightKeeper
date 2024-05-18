using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Material.Icons;
using SightKeeper.Application;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.MessageBoxDialog;
using SightKeeper.Avalonia.ViewModels;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia;

internal sealed partial class ExistingGameViewModel : GameViewModel
{
	public ExistingGameViewModel(
		Game game,
		GameIconProvider iconProvider,
		GameExecutableDisplayer executableDisplayer,
		DialogManager dialogManager,
		GamesDataAccess gamesDataAccess)
		: base(game, iconProvider, executableDisplayer)
	{
		_dialogManager = dialogManager;
		_gamesDataAccess = gamesDataAccess;
	}

	private readonly DialogManager _dialogManager;
	private readonly GamesDataAccess _gamesDataAccess;

	[RelayCommand]
	private async Task Delete()
	{
		MessageBoxButtonDefinition confirmButton = new("Delete", MaterialIconKind.TrashCanOutline);
		MessageBoxButtonDefinition cancelButton = new("Cancel", MaterialIconKind.Close, true);
		MessageBoxDialogViewModel messageBox = new(
			"Game deletion confirmation",
			$"Are you sure you want to delete \"{Game.Title}\" from the list of registered games?",
			confirmButton, cancelButton);
		var messageBoxResult = await _dialogManager.ShowDialogAsync(messageBox);
		if (messageBoxResult == confirmButton)
			_gamesDataAccess.RemoveGame(Game);
	}
}