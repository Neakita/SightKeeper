using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using Autofac;
using CommunityToolkit.Mvvm.ComponentModel;
using Material.Icons;
using ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Avalonia.ViewModels.Tabs;
using SightKeeper.Data;

namespace SightKeeper.Avalonia.ViewModels.Windows;

public sealed partial class MainViewModel : ViewModel, IActivatableViewModel
{
	public ViewModelActivator Activator { get; } = new();
	public ObservableCollection<TabItem> Tabs { get; } = new();
	public static AppDbContext DbContext { get; private set; }

	[ObservableProperty] private TabItem? _selectedTab;

	public MainViewModel(ILifetimeScope scope)
	{
		this.WhenActivated(disposables =>
		{
			var ownScope = scope.BeginLifetimeScope(this, builder => builder.RegisterInstance(scope.Resolve<AppDbContext>()));
			ownScope.DisposeWith(disposables);
			DbContext = ownScope.Resolve<AppDbContext>();
			var profilesViewModel = ownScope.Resolve<ProfilesViewModel>();
			var modelsViewModel = ownScope.Resolve<ModelsViewModel>();
			var annotatingTabViewModel = ownScope.Resolve<AnnotatingViewModel>();
			var settingsViewModel = ownScope.Resolve<SettingsViewModel>();
			Tabs.Add(new TabItem(MaterialIconKind.DotsGrid, "Profiles", profilesViewModel));
			Tabs.Add(new TabItem(MaterialIconKind.TableEye, "Models", modelsViewModel));
			Tabs.Add(new TabItem(MaterialIconKind.Image, "Annotating", annotatingTabViewModel));
			Tabs.Add(new TabItem(MaterialIconKind.Cog, "Settings", settingsViewModel));
			SelectedTab = Tabs.First();
			Disposable.Create(() =>
			{
				Tabs.Clear();
			}).DisposeWith(disposables);
		});
	}
}