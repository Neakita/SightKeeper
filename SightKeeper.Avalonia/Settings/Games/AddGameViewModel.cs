using System.Collections.Generic;
using System.Collections.Immutable;
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

internal sealed partial class AddGameViewModel : DialogViewModel<bool>, GameData, MutableGameData
{
	public override string Header => "Add game";

	protected override bool DefaultResult => false;

	public IReadOnlyCollection<GameViewModel> AvailableGames
	{
		get => _availableGames;
		private set => SetProperty(ref _availableGames, value);
	}

	public AddGameViewModel(
		ProcessesAvailableGamesProvider availableGamesProvider,
		GameIconProvider iconProvider,
		GameExecutableDisplayer executableDisplayer,
		IValidator<GameData> validator)
	{
		_availableGamesProvider = availableGamesProvider;
		_iconProvider = iconProvider;
		_executableDisplayer = executableDisplayer;
		_validator = new ViewModelValidator<GameData>(validator, this, this);
		_availableGames = GetAvailableGames();
	}

	private readonly ProcessesAvailableGamesProvider _availableGamesProvider;
	private readonly GameIconProvider _iconProvider;
	private readonly GameExecutableDisplayer _executableDisplayer;
	private readonly ViewModelValidator<GameData> _validator;
	private IReadOnlyCollection<GameViewModel> _availableGames;

	[ObservableProperty]
	private GameViewModel? _selectedGame;
	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(ApplyCommand))]
	private string _title = string.Empty;
	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(ApplyCommand))]
	private string _processName = string.Empty;
	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(ApplyCommand))]
	private string? _executablePath = string.Empty;

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

	[RelayCommand(CanExecute = nameof(CanApply))]
	private void Apply()
	{
		Guard.IsFalse(_validator.HasErrors);
		Return(true);
	}

	private bool CanApply()
	{
		return !_validator.HasErrors;
	}

	partial void OnSelectedGameChanged(GameViewModel? value)
	{
		using (_validator.SuppressValidation())
		{
			Title = value?.Title ?? string.Empty;
			ProcessName = value?.ProcessName ?? string.Empty;
			ExecutablePath = value?.ExecutablePath;
		}
		ApplyCommand.NotifyCanExecuteChanged();
	}
}