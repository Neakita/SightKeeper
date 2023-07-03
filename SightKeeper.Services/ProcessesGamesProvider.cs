using System.Diagnostics;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Services;

namespace SightKeeper.Services;

public sealed class ProcessesGamesProvider : AvailableGamesProvider
{
    public ProcessesGamesProvider(GamesDataAccess gamesDataAccess)
    {
        _gamesDataAccess = gamesDataAccess;
    }
    
    public async Task<IReadOnlyCollection<Game>> GetAvailableGamesAsync(CancellationToken cancellationToken = default)
    {
        var existing = await _gamesDataAccess.GetGamesAsync(cancellationToken);
        return Process.GetProcesses()
            .Where(process => process.MainWindowHandle != 0 && existing.Any(game => game.ProcessName == process.ProcessName))
            .Select(process => new Game(process.MainWindowTitle, process.ProcessName))
            .ToList();
    }

    private readonly GamesDataAccess _gamesDataAccess;
}