using System.Diagnostics;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Windows;

public class WindowsFileExplorerGameExecutableDisplayer : GameExecutableDisplayer
{
	public void ShowGameExecutable(Game game)
	{
		var executablePath = game.ExecutablePath;
		Guard.IsNotNull(executablePath);
		var argument = $"/select, \"{executablePath}\"";
		Process.Start("explorer.exe", argument);
	}
}