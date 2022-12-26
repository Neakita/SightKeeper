using SightKeeper.UI.WPF.ViewModels.Pages;

namespace SightKeeper.UI.WPF.Views.Pages;

public partial class ModelsPage
{
	internal ModelsPage(ModelsPageVM modelsPageVM)
	{
		InitializeComponent();
		DataContext = modelsPageVM;
	}
}