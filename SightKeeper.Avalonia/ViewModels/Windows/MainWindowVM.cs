using System.Collections.Generic;
using System.Linq;
using ReactiveUI.Fody.Helpers;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Avalonia.Views.Tabs;
using SightKeeper.Common;

namespace SightKeeper.Avalonia.ViewModels.Windows;

public class MainWindowVM : ViewModel
{
	public static MainWindowVM New => Locator.Resolve<MainWindowVM>();
	
	public ICollection<TabItem> Tabs { get; }

	[Reactive] public TabItem SelectedTab { get; set; }

	public MainWindowVM(ProfilesTab profilesTab, ModelsTab modelsTab, AnnotatingTab annotatingTab, SettingsTab settingsTab)
	{
		Tabs = new List<TabItem>
		{
			new("Profiles", profilesTab),
			new("Models", modelsTab),
			new("Annotating", annotatingTab),
			new("Settings", settingsTab)
		};
		
		SelectedTab = Tabs.First();
	}
}