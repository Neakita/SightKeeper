using System.Collections.Generic;
using System.Linq;
using Autofac;
using CommunityToolkit.Mvvm.ComponentModel;
using Material.Icons;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.ViewModels;
using SightKeeper.Avalonia.ViewModels.Elements;
using SettingsViewModel = SightKeeper.Avalonia.Settings.SettingsViewModel;

namespace SightKeeper.Avalonia;

internal partial class MainViewModel : ViewModel
{
	public DialogManager DialogManager { get; }
	public IReadOnlyCollection<TabItem> Tabs { get; }

	public MainViewModel(IComponentContext context, DialogManager dialogManager)
	{
		DialogManager = dialogManager;
		Tabs =
		[
			CreateTab<SettingsViewModel>(context, MaterialIconKind.Cog, "Settings"),
		];
		SelectedTab = Tabs.First();
	}

	[ObservableProperty] private TabItem _selectedTab;

	private static TabItem CreateTab<TViewModel>(
		IComponentContext context,
		MaterialIconKind iconKind,
		string header)
		where TViewModel : ViewModel
	{
		return new TabItem(iconKind, header, context.Resolve<TViewModel>());
	}
}