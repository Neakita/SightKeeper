using System.Diagnostics;
using SightKeeper.Application;
using SightKeeper.Domain.Model;

namespace SightKeeper.Services;

public sealed class ProcessesAvailableGamesProvider
{
	public IEnumerable<Game> AvailableGames => GetAvailableGames();

    public ProcessesAvailableGamesProvider(GamesDataAccess gamesDataAccess)
    {
        _gamesDataAccess = gamesDataAccess;
    }

    private IEnumerable<Game> GetAvailableGames()
    {
	    var existingGames = _gamesDataAccess.Games;
	    return Process.GetProcesses()
		    .Where(process => process.MainWindowHandle != 0 &&
		                      !string.IsNullOrWhiteSpace(process.MainWindowTitle) &&
		                      existingGames.All(game => game.ProcessName != process.ProcessName))
		    .Select(process => new Game(process.MainWindowTitle, process.ProcessName, process.MainModule?.FileName));
    }

    private readonly GamesDataAccess _gamesDataAccess;
}