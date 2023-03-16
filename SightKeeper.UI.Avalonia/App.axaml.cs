using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using SightKeeper.UI.Avalonia.ViewModels;
using SightKeeper.UI.Avalonia.ViewModels.Windows;
using SightKeeper.UI.Avalonia.Views;
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
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			desktop.MainWindow = new MainWindow
			{
				DataContext = new MainWindowViewModel(),
			};
		}

		base.OnFrameworkInitializationCompleted();
	}
}