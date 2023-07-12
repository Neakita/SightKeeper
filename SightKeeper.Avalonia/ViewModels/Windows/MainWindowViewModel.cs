using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Material.Icons;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Avalonia.ViewModels.Tabs;

namespace SightKeeper.Avalonia.ViewModels.Windows;

public sealed partial class MainWindowViewModel : ViewModel
{
	public ICollection<TabItem> Tabs { get; }

	[ObservableProperty] private TabItem _selectedTab;

	public MainWindowViewModel(
		ProfilesTabViewModel profilesTabViewModel,
		ModelsTabViewModel modelsTabViewModel,
		AnnotatingTabViewModel annotatingTabViewModel,
		SettingsViewModel settingsViewModel)
	{
		Tabs = new List<TabItem>
		{
			new(MaterialIconKind.DotsGrid, "Profiles", profilesTabViewModel),
			new(MaterialIconKind.TableEye, "Models", modelsTabViewModel),
			new(MaterialIconKind.Image, "Annotating", annotatingTabViewModel),
			new(MaterialIconKind.Cog, "Settings", settingsViewModel)
		};
		SelectedTab = Tabs.First();
	}
}