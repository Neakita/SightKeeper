using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Avalonia.Extensions;

public static class ControlExtensions
{
	public static void ReopenWindow<TWindow>() where TWindow : Window, new()
	{
		var application = global::Avalonia.Application.Current;
		Guard.IsNotNull(application);
		Guard.IsNotNull(application.ApplicationLifetime);
		var lifetime = (IClassicDesktopStyleApplicationLifetime)application.ApplicationLifetime;
		var window = lifetime.Windows.OfType<TWindow>().Single();
		TWindow replacement = new()
		{
			Position = window.Position,
			DataContext = window.DataContext,
			WindowState = window.WindowState,
			Width = window.Width,
			Height = window.Height
		};
		replacement.Show();
		window.Close();
	}
}