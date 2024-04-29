using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.ViewModels;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.Settings.Games;

internal sealed partial class AddGameViewModel : DialogViewModel<Game?>
{
	public override string Header => "Add game";

	protected override Game? DefaultResult => null;

	public IReadOnlyCollection<GameViewModel> AvailableGames
	{
		get => _availableGames;
		private set => SetProperty(ref _availableGames, value);
	}

	public AddGameViewModel(ProcessesAvailableGamesProvider availableGamesProvider, GameIconProvider iconProvider)
	{
		_availableGamesProvider = availableGamesProvider;
		_iconProvider = iconProvider;
		_availableGames = GetAvailableGames();
	}

	private readonly ProcessesAvailableGamesProvider _availableGamesProvider;
	private readonly GameIconProvider _iconProvider;
	private IReadOnlyCollection<GameViewModel> _availableGames;

	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(ApplyCommand))]
	private GameViewModel? _selectedGame;

	[RelayCommand]
	private void UpdateAvailableGames()
	{
		AvailableGames = GetAvailableGames();
	}

	private IReadOnlyCollection<GameViewModel> GetAvailableGames()
	{
		return _availableGamesProvider.AvailableGames.Select(CreateGameViewModel).ToImmutableArray();
	}

	private GameViewModel CreateGameViewModel(Game game)
	{
		return new GameViewModel(game, _iconProvider);
	}

	[RelayCommand(CanExecute = nameof(CanApply))]
	private void Apply()
	{
		Guard.IsNotNull(SelectedGame);
		Return(SelectedGame.Game);
	}

	private bool CanApply()
	{
		return SelectedGame != null;
	}
}