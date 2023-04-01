using SightKeeper.Infrastructure.Common;
using SightKeeper.UI.Avalonia.ViewModels.Elements;

namespace SightKeeper.UI.Avalonia.ViewModels.Tabs;

public sealed class SettingsTabVM
{
	public static SettingsTabVM New => Locator.Resolve<SettingsTabVM>();
	
	public RegisteredGamesVM RegisteredGamesVM { get; }
	
	public SettingsTabVM(RegisteredGamesVM registeredGamesVM)
	{
		RegisteredGamesVM = registeredGamesVM;
	}
}
