using System.Collections.Generic;
using System.Linq;
using Autofac;
using CommunityToolkit.Mvvm.ComponentModel;
using Material.Icons;
using SightKeeper.Avalonia.Annotation;
using SightKeeper.Avalonia.DataSets;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.Screenshots;
using SightKeeper.Avalonia.Settings.Appearance;
using SettingsViewModel = SightKeeper.Avalonia.Settings.SettingsViewModel;

namespace SightKeeper.Avalonia;

internal sealed partial class MainViewModel : ViewModel, DialogHost
{
	public DialogManager DialogManager { get; }
	public AppearanceSettingsViewModel AppearanceSettings { get; }
	public IReadOnlyCollection<TabItemViewModel> Tabs { get; }

	public MainViewModel(
		IComponentContext context,
		DialogManager dialogManager,
		AppearanceSettingsViewModel appearanceSettings)
	{
		DialogManager = dialogManager;
		AppearanceSettings = appearanceSettings;
		Tabs =
		[
			CreateTab<ScreenshotsLibrariesViewModel>(context, MaterialIconKind.FolderMultipleImage, "Screenshots"),
			CreateTab<DataSetsViewModel>(context, MaterialIconKind.ImageAlbum, "Datasets"),
			CreateTab<AnnotationTabViewModel>(context, MaterialIconKind.ImageEdit, "Annotation"),
			CreateTab<SettingsViewModel>(context, MaterialIconKind.Cog, "Settings")
		];
		SelectedTab = Tabs.First();
	}

	[ObservableProperty] private TabItemViewModel _selectedTab;

	private static TabItemViewModel CreateTab<TViewModel>(
		IComponentContext context,
		MaterialIconKind iconKind,
		string header)
		where TViewModel : ViewModel
	{
		return new TabItemViewModel(iconKind, header, context.Resolve<TViewModel>());
	}
}