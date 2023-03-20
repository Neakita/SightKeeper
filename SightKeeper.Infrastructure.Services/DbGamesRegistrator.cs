using System.Diagnostics;
using SightKeeper.Application;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Infrastructure.Data;

namespace SightKeeper.Infrastructure.Services;

public sealed class DbGamesRegistrator : GamesRegistrator
{
	private readonly AppDbContextFactory _dbContextFactory;
	public IReadOnlyCollection<Game> AvailableGames => _availableGames;
	public IReadOnlyCollection<Game> RegisteredGames => _registeredGames;

	public DbGamesRegistrator(AppDbContextFactory dbContextFactory)
	{
		_dbContextFactory = dbContextFactory;
		using AppDbContext dbContext = dbContextFactory.CreateDbContext();
		List<Game> gamesFromDb = dbContext.Games.ToList();
		_registeredGames = gamesFromDb;
		_registeredGamesByProcessNames = gamesFromDb.ToDictionary(game => game.ProcessName);
		_availableGames = GetAvailableGames();
	}

	public bool IsGameRegistered(Game game) => _registeredGamesByProcessNames.ContainsKey(game.ProcessName);

	public void RegisterGame(Game game)
	{
		if (IsGameRegistered(game)) throw new InvalidOperationException($"Game {game} already registered");

		using AppDbContext dbContext = _dbContextFactory.CreateDbContext();
		dbContext.Games.Add(game);
		dbContext.SaveChanges();

		if (_availableGames.Contains(game)) _availableGames.Remove(game);
		else _availableGames.RemoveAll(availableGame => availableGame.ProcessName == game.ProcessName);
		_registeredGames.Add(game);
		_registeredGamesByProcessNames.Add(game.ProcessName, game);
	}

	public void UnregisterGame(Game game)
	{
		if (!IsGameRegistered(game)) throw new InvalidOperationException($"Game {game} is not registered");
		
		using AppDbContext dbContext = _dbContextFactory.CreateDbContext();
		dbContext.Games.Remove(game);
		dbContext.SaveChanges();
		
		_registeredGames.Remove(game);
		_registeredGamesByProcessNames.Remove(game.ProcessName);
		game.Id = -1;
		_availableGames.Add(game);
	}

	public void RefreshAvailableGames() => _availableGames = GetAvailableGames();

	private List<Game> _availableGames;
	private readonly List<Game> _registeredGames;
	private readonly Dictionary<string, Game> _registeredGamesByProcessNames;

	private List<Game> GetAvailableGames() => Process.GetProcesses()
			.Where(process => process.MainWindowHandle != 0 && !_registeredGamesByProcessNames.ContainsKey(process.ProcessName))
			.Select(process => new Game(process.MainWindowTitle, process.ProcessName))
			.ToList();
}