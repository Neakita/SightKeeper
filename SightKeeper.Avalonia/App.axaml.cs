using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace SightKeeper.Avalonia;

internal sealed class App : global::Avalonia.Application
{
	public override void Initialize()
	{
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted()
	{
		var composition = AppBootstrapper.Setup();
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
			desktopLifetime.MainWindow = composition.MainWindow;
		if (ApplicationLifetime is IControlledApplicationLifetime controlledLifetime)
			controlledLifetime.Exit += (_, _) => composition.Dispose();
		base.OnFrameworkInitializationCompleted();
	}
}