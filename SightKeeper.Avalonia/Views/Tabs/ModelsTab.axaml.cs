using Avalonia.ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Tabs;

namespace SightKeeper.Avalonia.Views.Tabs;

public sealed partial class ModelsTab : ReactiveUserControl<ModelsViewModel>
{
	public ModelsTab()
	{
		InitializeComponent();
	}
}