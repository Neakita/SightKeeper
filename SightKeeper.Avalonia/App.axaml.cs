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
		switch (ApplicationLifetime)
		{
			case IClassicDesktopStyleApplicationLifetime desktop:
				desktop.MainWindow = Composition.MainWindow;
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(ApplicationLifetime), ApplicationLifetime, null);
		}
		if (ApplicationLifetime is IControlledApplicationLifetime controlledApplicationLifetime)
			controlledApplicationLifetime.Exit += (_, _) => Composition.Dispose();

		base.OnFrameworkInitializationCompleted();
	}
}