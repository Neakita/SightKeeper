using Autofac;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.Setup;

namespace SightKeeper.Avalonia;

public sealed class App : global::Avalonia.Application
{
	public override void Initialize()
	{
		AvaloniaXamlLoader.Load(this);
		DataTemplates.AddRange(ViewsBootstrapper.GetDataTemplates());
	}

	public override void OnFrameworkInitializationCompleted()
	{
		if (ApplicationLifetime is IControlledApplicationLifetime controlledLifetime)
			controlledLifetime.Exit += OnExit;
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			var scope = ServiceLocator.Instance.BeginLifetimeScope(builder =>
			{
				builder.RegisterInstance(new DialogManager());
			});
			var viewModel = scope.Resolve<MainViewModel>();
			desktop.MainWindow = new MainWindow
			{
				DataContext = viewModel
			};
		}
		base.OnFrameworkInitializationCompleted();
	}

	private void OnExit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
	{
		if (ApplicationLifetime is IControlledApplicationLifetime controlledLifetime)
			controlledLifetime.Exit -= OnExit;
		AppBootstrapper.OnRelease();
		ServiceLocator.Instance.Dispose();
	}
}