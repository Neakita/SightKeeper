using Avalonia.ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Windows;

namespace SightKeeper.Avalonia.Views.Windows;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
	public MainWindow()
	{
		InitializeComponent();
	}
	
	public MainWindow(MainWindowViewModel viewModel)
	{
		InitializeComponent();
		ViewModel = viewModel;
	}
}