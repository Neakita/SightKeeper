using Avalonia.ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Tabs;

namespace SightKeeper.Avalonia.Views.Tabs;

public partial class ModelsTab : ReactiveUserControl<ModelsViewModel>
{
	public ModelsTab(ModelsViewModel viewModel) : this()
	{
		ViewModel = viewModel;
	}

	public ModelsTab()
	{
		InitializeComponent();
	}
}