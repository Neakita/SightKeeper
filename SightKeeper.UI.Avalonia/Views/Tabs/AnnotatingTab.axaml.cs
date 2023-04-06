using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SightKeeper.UI.Avalonia.ViewModels.Tabs;

namespace SightKeeper.UI.Avalonia.Views.Tabs;

public partial class AnnotatingTab : ReactiveUserControl<AnnotatingTabVM>
{
	public AnnotatingTab(AnnotatingTabVM viewModel) : this()
	{
		ViewModel = viewModel;
	}
	
	public AnnotatingTab()
	{
		InitializeComponent();
		KeyDown += OnKeyDown;
	}

	private void OnKeyDown(object? sender, KeyEventArgs e) => ViewModel?.OnKeyDown(e.Key);

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
	}
}