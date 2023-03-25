using Avalonia;
using Avalonia.ReactiveUI;
using SightKeeper.UI.Avalonia.ViewModels.Windows;

namespace SightKeeper.UI.Avalonia.Views.Windows;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
	public MainWindow()
	{
		InitializeComponent();
#if DEBUG
		this.AttachDevTools();
#endif
	}
	
	public MainWindow(MainWindowViewModel viewModel)
	{
		InitializeComponent();
		ViewModel = viewModel;
	}
}