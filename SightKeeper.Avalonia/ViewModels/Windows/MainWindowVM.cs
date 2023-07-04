using System.Collections.Generic;
using System.Linq;
using Material.Icons;
using ReactiveUI.Fody.Helpers;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Avalonia.Views.Tabs;

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
			new(MaterialIconKind.DotsGrid, "Profiles", profilesTab),
			new(MaterialIconKind.TableEye, "Models", modelsTab),
			new(MaterialIconKind.Image, "Annotating", annotatingTab),
			new(MaterialIconKind.Cog, "Settings", settingsTab)
		};
		
		SelectedTab = Tabs.First();
	}
}