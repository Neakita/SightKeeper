using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.ViewModels;

namespace SightKeeper.Avalonia.Settings.Games;

internal sealed partial class GamesSettingsViewModel : ViewModel, SettingsSection
{
	public string Header => "Games";
	public IReadOnlyCollection<GameViewModel> Games { get; }

	public GamesSettingsViewModel(
		IComponentContext context,
		GamesRepositoryViewModel gamesRepository,
		DialogManager dialogManager,
		GameCreator gameCreator)
	{
		_context = context;
		_dialogManager = dialogManager;
		_gameCreator = gameCreator;
		Games = gamesRepository.Games;
	}

	private readonly IComponentContext _context;
	private readonly DialogManager _dialogManager;
	private readonly GameCreator _gameCreator;

	[RelayCommand]
	private async Task AddGame()
	{
		var viewModel = _context.Resolve<AddGameViewModel>();
		var applied = await _dialogManager.ShowDialogAsync(viewModel);
		if (applied)
			_gameCreator.CreateGame(viewModel);
	}
}