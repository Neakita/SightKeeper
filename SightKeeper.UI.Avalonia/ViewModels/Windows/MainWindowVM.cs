using System.Collections.Generic;
using System.Linq;
using ReactiveUI.Fody.Helpers;
using SightKeeper.UI.Avalonia.ViewModels.Elements;
using SightKeeper.UI.Avalonia.Views.Tabs;

namespace SightKeeper.UI.Avalonia.ViewModels.Windows;

public class MainWindowVM : ViewModel
{
	public ICollection<TabItemViewModel> Tabs { get; }

	[Reactive] public TabItemViewModel SelectedTab { get; set; }


	public MainWindowVM(ProfilesTab profilesTab, ModelsTab modelsTab, SettingsTab settingsTab)
	{
		Tabs = new List<TabItemViewModel>
		{
			new("Profiles", profilesTab),
			new("Models", modelsTab),
			new("Settings", settingsTab)
		};
		
		SelectedTab = Tabs.First();
	}
}