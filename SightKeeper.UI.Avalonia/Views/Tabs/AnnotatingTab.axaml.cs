using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SightKeeper.UI.Avalonia.ViewModels.Tabs;

namespace SightKeeper.UI.Avalonia.Views.Tabs;

public partial class AnnotatingTab : ReactiveUserControl<AnnotatingTabViewModel>
{
	public AnnotatingTab(AnnotatingTabViewModel viewModel) : this()
	{
		ViewModel = viewModel;
	}
	
	public AnnotatingTab()
	{
		InitializeComponent();
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
	}
}