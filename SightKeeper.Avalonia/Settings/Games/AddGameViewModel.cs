using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using SightKeeper.Application;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.ViewModels;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.Settings.Games;

internal sealed partial class AddGameViewModel : DialogViewModel<Game?>
{
	public override string Header => "Add game";

	public ValidatableGameViewModel? GameToAdd
	{
		get => _gameToAdd;
		set
		{
			if (_gameToAdd != null)
				_gameToAdd.ErrorsChanged -= OnGameErrorsChanged;
			_gameToAdd = value;
			if (_gameToAdd != null)
				_gameToAdd.ErrorsChanged += OnGameErrorsChanged;
			OnPropertyChanged();
			ApplyCommand.NotifyCanExecuteChanged();
		}
	}

	private void OnGameErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
	{
		ApplyCommand.NotifyCanExecuteChanged();
	}

	protected override Game? DefaultResult => null;

	public IReadOnlyCollection<GameViewModel> AvailableGames
	{
		get => _availableGames;
		private set => SetProperty(ref _availableGames, value);
	}

	public AddGameViewModel(
		ProcessesAvailableGamesProvider availableGamesProvider,
		GameIconProvider iconProvider,
		GameExecutableDisplayer executableDisplayer,
		IValidator<Game> validator)
	{
		_availableGamesProvider = availableGamesProvider;
		_iconProvider = iconProvider;
		_executableDisplayer = executableDisplayer;
		_validator = validator;
		_availableGames = GetAvailableGames();
	}

	private readonly ProcessesAvailableGamesProvider _availableGamesProvider;
	private readonly GameIconProvider _iconProvider;
	private readonly GameExecutableDisplayer _executableDisplayer;
	private readonly IValidator<Game> _validator;
	private IReadOnlyCollection<GameViewModel> _availableGames;

	[ObservableProperty]
	private GameViewModel? _selectedGame;
	private ValidatableGameViewModel? _gameToAdd;

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
		return new GameViewModel(game, _iconProvider, _executableDisplayer);
	}

	private ValidatableGameViewModel CreateValidatableGameViewModel(Game game)
	{
		return new ValidatableGameViewModel(game, _iconProvider, _executableDisplayer, _validator);
	}

	[RelayCommand(CanExecute = nameof(CanApply))]
	private void Apply()
	{
		Guard.IsNotNull(GameToAdd);
		Return(GameToAdd.Game);
	}

	private bool CanApply()
	{
		return GameToAdd is { HasErrors: false };
	}

	partial void OnSelectedGameChanged(GameViewModel? value)
	{
		if (value == null)
			GameToAdd = null;
		else
			GameToAdd = CreateValidatableGameViewModel(value.Game);
	}

	protected override void Return(Game? result)
	{
		GameToAdd = null;
		base.Return(result);
	}
}