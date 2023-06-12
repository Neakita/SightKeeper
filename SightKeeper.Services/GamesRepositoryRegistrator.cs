using System.Diagnostics;
using SightKeeper.Application;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Services;

namespace SightKeeper.Services;

public sealed class GamesRepositoryRegistrator : GamesRegistrator
{
	private readonly Repository<Game> _repository;
	public IReadOnlyCollection<Game> AvailableGames => _availableGames;
	public IReadOnlyCollection<Game> RegisteredGames => _repository.Items;

	public GamesRepositoryRegistrator(Repository<Game> repository)
	{
		_repository = repository;
		_registeredGamesByProcessNames = repository.Items.ToDictionary(game => game.ProcessName);
		_availableGames = GetAvailableGames();
	}

	public bool IsGameRegistered(Game game) => _registeredGamesByProcessNames.ContainsKey(game.ProcessName);

	public void RegisterGame(Game game)
	{
		if (IsGameRegistered(game)) throw new InvalidOperationException($"Game {game} already registered");

		if (_availableGames.Contains(game)) _availableGames.Remove(game);
		else _availableGames.RemoveAll(availableGame => availableGame.ProcessName == game.ProcessName);
		_repository.Add(game);
		_registeredGamesByProcessNames.Add(game.ProcessName, game);
	}

	public void UnregisterGame(Game game)
	{
		if (!IsGameRegistered(game)) throw new InvalidOperationException($"Game {game} is not registered");
		
		_repository.Remove(game);
		_registeredGamesByProcessNames.Remove(game.ProcessName);
		_availableGames.Add(game);
	}

	public void RefreshAvailableGames() => _availableGames = GetAvailableGames();

	private List<Game> _availableGames;
	private readonly Dictionary<string, Game> _registeredGamesByProcessNames;

	private List<Game> GetAvailableGames() => Process.GetProcesses()
		.Where(process => process.MainWindowHandle != 0 &&
		                  !_registeredGamesByProcessNames.ContainsKey(process.ProcessName) &&
		                  process.MainWindowTitle != string.Empty)
		.Select(process => new Game(process.MainWindowTitle, process.ProcessName))
		.ToList();
}