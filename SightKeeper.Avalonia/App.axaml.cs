using System;
using Autofac;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using SightKeeper.Avalonia.Setup;

namespace SightKeeper.Avalonia;

public sealed class App : global::Avalonia.Application, IDisposable
{
	public override void Initialize()
	{
		AvaloniaXamlLoader.Load(this);
		DataTemplates.AddRange(ViewsBootstrapper.GetDataTemplates());
	}

	public override void OnFrameworkInitializationCompleted()
	{
		_container = AppBootstrapper.Setup();
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			desktop.MainWindow = new MainWindow
			{
				DataContext = _container.Resolve<MainViewModel>()
			};
		}
		base.OnFrameworkInitializationCompleted();
	}

	public void Dispose()
	{
		_container?.Dispose();
	}

	private IContainer? _container;
}