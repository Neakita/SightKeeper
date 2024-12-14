using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.Settings.Appearance;

namespace SightKeeper.Avalonia;

internal sealed partial class MainViewModel : ViewModel, DialogHost
{
	public DialogManager DialogManager { get; }
	public AppearanceSettingsViewModel AppearanceSettings { get; }
	public IReadOnlyCollection<TabItemViewModel> Tabs { get; }

	public MainViewModel(
		DialogManager dialogManager,
		AppearanceSettingsViewModel appearanceSettings,
		IReadOnlyCollection<TabItemViewModel> tabs)
	{
		DialogManager = dialogManager;
		AppearanceSettings = appearanceSettings;
		Tabs = tabs;
		SelectedTab = Tabs.First();
	}

	[ObservableProperty] private TabItemViewModel _selectedTab;
}