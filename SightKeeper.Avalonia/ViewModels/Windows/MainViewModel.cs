using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using Autofac;
using CommunityToolkit.Mvvm.ComponentModel;
using Material.Icons;
using ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Annotating;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Avalonia.ViewModels.Tabs;

namespace SightKeeper.Avalonia.ViewModels.Windows;

public sealed partial class MainViewModel : ViewModel, IActivatableViewModel
{
	public ViewModelActivator Activator { get; } = new();
	public ObservableCollection<TabItem> Tabs { get; } = new();

	[ObservableProperty] private TabItem? _selectedTab;

	public MainViewModel(ILifetimeScope scope)
	{
		this.WhenActivated(disposables =>
		{
			var ownScope = scope.BeginLifetimeScope(typeof(MainViewModel));
			ownScope.DisposeWith(disposables);
			var profilesViewModel = ownScope.Resolve<ProfilesViewModel>();
			var dataSetsViewModel = ownScope.Resolve<DataSetsViewModel>();
			var annotatingViewModel = ownScope.Resolve<AnnotatorViewModel>();
			var trainingViewModel = ownScope.Resolve<TrainingViewModel>();
			var settingsViewModel = ownScope.Resolve<SettingsViewModel>();
			Tabs.Add(new TabItem(MaterialIconKind.DotsGrid, "Profiles", profilesViewModel));
			Tabs.Add(new TabItem(MaterialIconKind.TableEye, "Datasets", dataSetsViewModel));
			Tabs.Add(new TabItem(MaterialIconKind.Image, "Annotating", annotatingViewModel));
			Tabs.Add(new TabItem(MaterialIconKind.Abacus, "Training", trainingViewModel));
			Tabs.Add(new TabItem(MaterialIconKind.Cog, "Settings", settingsViewModel));
			SelectedTab = Tabs.First();
			Disposable.Create(() =>
			{
				Tabs.Clear();
			}).DisposeWith(disposables);
		});
	}
}