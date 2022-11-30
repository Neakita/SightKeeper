using SightKeeper.UI.WPF.ViewModels.Windows;

namespace SightKeeper.UI.WPF.Views.Windows;

public partial class MainWindow
{
	public MainWindow(MainWindowVM viewModel)
	{
		InitializeComponent();
		DataContext = viewModel;
	}
}