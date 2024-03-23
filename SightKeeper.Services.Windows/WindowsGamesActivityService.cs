using System.Diagnostics;
using SightKeeper.Application;
using SightKeeper.Domain.Model;

namespace SightKeeper.Services.Windows;

public sealed class WindowsGamesActivityService : GamesActivityService
{
    public bool IsGameActive(Game game)
    {
        var process = Process.GetProcessesByName(game.ProcessName)
            .FirstOrDefault(process => process.MainWindowHandle > 0);
        if (process == null)
            return false;
        return User32.GetForegroundWindow() == process.MainWindowHandle;
    }
}