using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Avalonia;

internal sealed class App : global::Avalonia.Application
{
	public static IStorageProvider StorageProvider
	{
		get
		{
			var lifetime = Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
			Guard.IsNotNull(lifetime);
			var window = lifetime.MainWindow;
			Guard.IsNotNull(window);
			return window.StorageProvider;
		}
	}

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