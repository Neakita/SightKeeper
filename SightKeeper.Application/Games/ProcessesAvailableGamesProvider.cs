using System.Diagnostics;
using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Games;

public sealed class ProcessesAvailableGamesProvider
{
	public IEnumerable<Game> AvailableGames => GetAvailableGames();

    public ProcessesAvailableGamesProvider(ReadDataAccess<Game> gamesDataAccess)
    {
        _gamesDataAccess = gamesDataAccess;
    }

    private IEnumerable<Game> GetAvailableGames()
    {
	    return Process.GetProcesses()
		    .Where(IsValidProcess)
		    .ExceptBy(_gamesDataAccess.Items.Select(ProcessDescription.Extract), ProcessDescription.Extract)
		    .Select(process => new Game(process.MainWindowTitle, process.ProcessName, process.MainModule?.FileName ?? string.Empty));
    }

    private readonly ReadDataAccess<Game> _gamesDataAccess;

    private static bool IsValidProcess(Process process) =>
	    process.MainWindowHandle != IntPtr.Zero &&
	    !string.IsNullOrEmpty(process.MainWindowTitle) &&
	    process.Id != Environment.ProcessId &&
	    CanGetMainModule(process);

    private static bool CanGetMainModule(Process process)
    {
	    try
	    {
		    _ = process.MainModule;
	    }
	    catch
	    {
		    return false;
	    }
	    return true;
    }
}