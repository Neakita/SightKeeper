using Avalonia;
using Avalonia.ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Windows;

namespace SightKeeper.Avalonia.Views.Windows;

public partial class MainWindow : ReactiveWindow<MainWindowVM>
{
	public MainWindow()
	{
		InitializeComponent();
#if DEBUG
		this.AttachDevTools();
#endif
	}
	
	public MainWindow(MainWindowVM vm)
	{
		InitializeComponent();
		ViewModel = vm;
	}
}