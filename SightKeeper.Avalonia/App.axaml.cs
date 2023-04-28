using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using SightKeeper.Common;
using SightKeeper.Services.Input;

namespace SightKeeper.Avalonia;

public class App : global::Avalonia.Application
{
	public override void Initialize()
	{
		AppBootstrapper.Setup();
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted()
	{
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			desktop.Exit += Shutdown;
			desktop.MainWindow = Locator.Resolve<Views.Windows.MainWindow>();
		}

		base.OnFrameworkInitializationCompleted();
	}

	private void Shutdown(object? sender, ControlledApplicationLifetimeExitEventArgs e)
	{
		Locator.Resolve<SharpHookHotKeyManager>().Dispose();
	}
}