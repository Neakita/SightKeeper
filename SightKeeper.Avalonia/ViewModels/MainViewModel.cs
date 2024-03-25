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

	public MainViewModel(ILifetimeScope scope)
	{
		Tabs =
		[
			CreateTab<ProfilesViewModel>(scope, MaterialIconKind.Puzzle, "Profiles"),
			CreateTab<DataSetsViewModel>(scope, MaterialIconKind.TableEye, "Datasets"),
			CreateTab<AnnotatorViewModel>(scope, MaterialIconKind.Image, "Annotating"),
			CreateTab<TrainingViewModel>(scope, MaterialIconKind.Abacus, "Training"),
			CreateTab<SettingsViewModel>(scope, MaterialIconKind.Cog, "Settings"),
		];
		SelectedTab = Tabs.First();
	}

	private static TabItem CreateTab<TViewModel>(IComponentContext context, MaterialIconKind iconKind, string header)
		where TViewModel : ViewModel
	{
		return new TabItem(iconKind, header, context.Resolve<TViewModel>());
	}
}