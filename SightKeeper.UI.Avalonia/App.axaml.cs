using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using SightKeeper.Infrastructure.Common;
using MainWindow = SightKeeper.UI.Avalonia.Views.Windows.MainWindow;

namespace SightKeeper.UI.Avalonia;

public partial class App : Application
{
	public override void Initialize()
	{
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted()
	{
		AppBootstrapper.Setup();
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			desktop.MainWindow = Locator.Resolve<MainWindow>();
		}
		else throw new Exception($"Unexpected environment: {ApplicationLifetime}");

		base.OnFrameworkInitializationCompleted();
	}
}