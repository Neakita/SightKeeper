using Autofac;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace SightKeeper.Avalonia;

public sealed class App : global::Avalonia.Application
{
	public override void Initialize()
	{
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted()
	{
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			desktop.MainWindow = ServiceLocator.Instance.Resolve<Views.Windows.MainWindow>();
		}

		base.OnFrameworkInitializationCompleted();
	}
}