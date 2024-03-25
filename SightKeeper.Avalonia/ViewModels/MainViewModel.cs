using System.Collections.ObjectModel;
using System.Linq;
using Autofac;
using CommunityToolkit.Mvvm.ComponentModel;
using Material.Icons;
using ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Annotating;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Avalonia.ViewModels.Tabs;
using SightKeeper.Avalonia.ViewModels.Tabs.Profiles;

namespace SightKeeper.Avalonia.ViewModels;

public sealed partial class MainViewModel : ViewModel
{
	public ViewModelActivator Activator { get; } = new();
	public ObservableCollection<TabItem> Tabs { get; }

	[ObservableProperty] private TabItem? _selectedTab;

	public MainViewModel(IComponentContext context)
	{
		Tabs =
		[
			CreateTab<ProfilesViewModel>(context, MaterialIconKind.Puzzle, "Profiles"),
			CreateTab<DataSetsViewModel>(context, MaterialIconKind.TableEye, "Datasets"),
			CreateTab<AnnotatorViewModel>(context, MaterialIconKind.Image, "Annotating"),
			CreateTab<TrainingViewModel>(context, MaterialIconKind.Abacus, "Training"),
			CreateTab<SettingsViewModel>(context, MaterialIconKind.Cog, "Settings"),
		];
		SelectedTab = Tabs.First();
	}

	private static TabItem CreateTab<TViewModel>(IComponentContext context, MaterialIconKind iconKind, string header)
		where TViewModel : ViewModel
	{
		return new TabItem(iconKind, header, context.Resolve<TViewModel>());
	}
}