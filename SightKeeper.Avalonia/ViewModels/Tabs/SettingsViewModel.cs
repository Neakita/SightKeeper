using System.Reactive.Disposables;
using Autofac;
using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Elements;

namespace SightKeeper.Avalonia.ViewModels.Tabs;

public sealed partial class SettingsViewModel : ViewModel, IActivatableViewModel
{
	public ViewModelActivator Activator { get; } = new();
	[ObservableProperty] private RegisteredGamesViewModel? _registeredGamesViewModel;
	
	public SettingsViewModel(ILifetimeScope scope)
	{
		_scope = scope;
		this.WhenActivated(HandleActivation);
	}

	private void HandleActivation(CompositeDisposable disposables)
	{
		var scope = _scope.BeginLifetimeScope(this);
		scope.DisposeWith(disposables);
		RegisteredGamesViewModel = scope.Resolve<RegisteredGamesViewModel>();
	}
	
	private readonly ILifetimeScope _scope;
}
