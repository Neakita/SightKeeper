using ReactiveUI;
using SightKeeper.UI.WPF.ViewModels.Windows;

namespace SightKeeper.UI.WPF.Views.Windows;

public partial class MainWindow : IViewFor<MainWindowVM>
{
	public MainWindow()
	{
		InitializeComponent();
		this.OneWayBind(ViewModel, vm => vm.Str, view => view.TextBlock.Text);
	}

	object? IViewFor.ViewModel
	{
		get => ViewModel;
		set => ViewModel = (MainWindowVM?) value;
	}

	public MainWindowVM? ViewModel { get; set; }
}