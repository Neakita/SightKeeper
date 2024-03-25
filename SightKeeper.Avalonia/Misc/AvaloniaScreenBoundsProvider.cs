using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform;
using SightKeeper.Application;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.Misc;

public sealed class AvaloniaScreenBoundsProvider : ScreenBoundsProvider
{
	public Vector2<int> MainScreenSize => new(PrimaryScreenBounds.Size.Width, PrimaryScreenBounds.Size.Height);

	private PixelRect PrimaryScreenBounds => _primaryScreenBounds ??= GetPrimaryScreenBounds();
	private PixelRect? _primaryScreenBounds;

	private static PixelRect GetPrimaryScreenBounds()
	{
		if (global::Avalonia.Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime
		    desktop)
			throw new InvalidOperationException(
				$"ApplicationLifetime is not {nameof(IClassicDesktopStyleApplicationLifetime)}");
		Window mainWindow = desktop.MainWindow ??
		                    throw new InvalidOperationException("Application main window is not initialized");
		Screen primaryScreen = mainWindow.Screens.Primary ?? throw new InvalidOperationException("Primary screen not found");
		return primaryScreen.Bounds;
	}
}