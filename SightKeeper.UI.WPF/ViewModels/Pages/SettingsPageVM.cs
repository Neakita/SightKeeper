using System;
using System.Diagnostics;
using System.Linq;
using ReactiveUI;
using SightKeeper.Backend.Data;
using SightKeeper.Backend.Data.Members;

namespace SightKeeper.UI.WPF.ViewModels.Pages;

public class SettingsPageVM : ReactiveObject
{
	public Game[] Games
	{
		get
		{
			using AppDbContext dbContext = new();
			return Games.ToArray();
		}
	}

	public Process[] Processes => Process.GetProcesses().Where(process => process.MainWindowHandle != IntPtr.Zero).ToArray();
}
