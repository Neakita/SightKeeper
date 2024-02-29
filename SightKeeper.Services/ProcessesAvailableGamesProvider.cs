using System.Diagnostics;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Services;

public sealed class ProcessesAvailableGamesProvider : AvailableGamesProvider
{
    public ProcessesAvailableGamesProvider(GamesDataAccess gamesDataAccess)
    {
        _gamesDataAccess = gamesDataAccess;
    }
    
    public async Task<IReadOnlyCollection<Game>> GetAvailableGames(CancellationToken cancellationToken = default)
    {
        var existing = await _gamesDataAccess.GetGames(cancellationToken);
        return await Task.Run(() =>
        {
            return Process.GetProcesses()
                .Where(process => process.MainWindowHandle != 0 &&
                                  !string.IsNullOrWhiteSpace(process.MainWindowTitle) &&
                                  existing.All(game => game.ProcessName != process.ProcessName))
                .Select(process => new Game(process.MainWindowTitle, process.ProcessName))
                .ToList();
        }, cancellationToken);
    }

    private readonly GamesDataAccess _gamesDataAccess;
}