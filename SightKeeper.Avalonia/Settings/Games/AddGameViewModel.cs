using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using SightKeeper.Avalonia.Dialogs;

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
		IValidator<GameData> validator,
		GameIconProvider? iconProvider = null,
		GameExecutableDisplayer? executableDisplayer = null)
	{
		_availableGamesProvider = availableGamesProvider;
		_iconProvider = iconProvider;
		_executableDisplayer = executableDisplayer;
		_validator = new ViewModelValidator<GameData>(validator, this, this);
		_availableGames = ImmutableList<GameViewModel>.Empty;
	}

	[RelayCommand]
	public async Task UpdateAvailableGames()
	{
		AvailableGames = await GetAvailableGamesAsync();
	}

	private readonly ProcessesAvailableGamesProvider _availableGamesProvider;
	private readonly GameIconProvider? _iconProvider;
	private readonly GameExecutableDisplayer? _executableDisplayer;
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

	private Task<IReadOnlyCollection<GameViewModel>> GetAvailableGamesAsync()
	{
		return Task.Run(GetAvailableGames);
	}

	private IReadOnlyCollection<GameViewModel> GetAvailableGames()
	{
		return _availableGamesProvider.AvailableGames.Select(CreateGameViewModel).ToImmutableList();
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