using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SightKeeper.UI.Avalonia.ViewModels.Tabs;

namespace SightKeeper.UI.Avalonia.Views.Tabs;

public partial class AnnotatingTab : ReactiveUserControl<AnnotatingTabVM>
{
	public AnnotatingTab(AnnotatingTabVM vm) : this()
	{
		ViewModel = vm;
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