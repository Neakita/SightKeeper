using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using SightKeeper.Infrastructure.Common;
using MainWindow = SightKeeper.UI.Avalonia.Views.Windows.MainWindow;

namespace SightKeeper.UI.Avalonia;

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
			desktop.MainWindow = Locator.Resolve<MainWindow>();
		base.OnFrameworkInitializationCompleted();
	}
}