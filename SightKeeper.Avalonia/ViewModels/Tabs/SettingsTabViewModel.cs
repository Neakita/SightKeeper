using SightKeeper.Avalonia.ViewModels.Elements;

namespace SightKeeper.Avalonia.ViewModels.Tabs;

public sealed class SettingsTabViewModel : ViewModel
{
	public RegisteredGamesViewModel RegisteredGamesViewModel { get; }
	
	public SettingsTabViewModel(RegisteredGamesViewModel registeredGamesViewModel)
	{
		RegisteredGamesViewModel = registeredGamesViewModel;
	}
}
