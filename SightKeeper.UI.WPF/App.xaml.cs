using System;
using SightKeeper.UI.WPF.Views.Windows;
using Splat;

namespace SightKeeper.UI.WPF;

public partial class App
{
	public App()
	{
		InitializeComponent();
		AppBootstrapper.Setup();
		MainWindow? mainWindow = Locator.Current.GetService<MainWindow>();
		if (mainWindow == null) throw new InvalidOperationException("MainWindow is not initialized");
		mainWindow.Show();
	}
}