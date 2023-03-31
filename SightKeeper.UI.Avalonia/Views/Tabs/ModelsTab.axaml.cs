using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SightKeeper.UI.Avalonia.ViewModels.Tabs;

namespace SightKeeper.UI.Avalonia.Views.Tabs;

public partial class ModelsTab : ReactiveUserControl<ModelsTabVM>
{
	public ModelsTab(ModelsTabVM vm) : this()
	{
		ViewModel = vm;
	}

	public ModelsTab()
	{
		InitializeComponent();
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
	}
}