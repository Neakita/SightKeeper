using System;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace SightKeeper.Avalonia;

internal sealed class App : global::Avalonia.Application
{
	public Composition Composition =>
		(Composition?)Resources[nameof(Composition)] ??
		throw new NullReferenceException("Unable to get Composition from application resources");

	public override void Initialize()
	{
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted()
	{
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
			desktopLifetime.MainWindow = Composition.MainWindow;
		if (ApplicationLifetime is IControlledApplicationLifetime controlledLifetime)
			controlledLifetime.Exit += (_, _) => Composition.Dispose();
		base.OnFrameworkInitializationCompleted();
	}
}