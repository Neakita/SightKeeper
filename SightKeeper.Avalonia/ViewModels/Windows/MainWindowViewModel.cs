using System.Collections.Generic;
using System.Linq;
using ReactiveUI.Fody.Helpers;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Avalonia.Views.Tabs;
using SightKeeper.Common;

namespace SightKeeper.Avalonia.ViewModels.Windows;

public class MainWindowViewModel : ViewModel
{
	public static MainWindowViewModel New => Locator.Resolve<MainWindowViewModel>();
	
	public ICollection<TabItemViewModel> Tabs { get; }

	[Reactive] public TabItemViewModel SelectedTab { get; set; }

	public MainWindowViewModel(ProfilesTab profilesTab, ModelsTab modelsTab, AnnotatingTab annotatingTab, SettingsTab settingsTab)
	{
		Tabs = new List<TabItemViewModel>
		{
			new("Profiles", profilesTab),
			new("Models", modelsTab),
			new("Annotating", annotatingTab),
			new("Settings", settingsTab)
		};
		
		SelectedTab = Tabs.First();
	}
}