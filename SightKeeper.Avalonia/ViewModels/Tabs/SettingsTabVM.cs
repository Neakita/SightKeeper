using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Common;

namespace SightKeeper.Avalonia.ViewModels.Tabs;

public sealed class SettingsTabVM
{
	public static SettingsTabVM New => Locator.Resolve<SettingsTabVM>();
	
	public RegisteredGamesVM RegisteredGamesVM { get; }
	
	public SettingsTabVM(RegisteredGamesVM registeredGamesVM)
	{
		RegisteredGamesVM = registeredGamesVM;
	}
}
