using Avalonia.Controls;
using SightKeeper.UI.Avalonia.ViewModels.Windows;

namespace SightKeeper.UI.Avalonia.Views.Windows;

public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
	}
	
	public MainWindow(MainWindowVM viewModel)
	{
		InitializeComponent();
		DataContext = viewModel;
	}
}