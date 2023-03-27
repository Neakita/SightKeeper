using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SightKeeper.UI.Avalonia.Misc;
using SightKeeper.UI.Avalonia.ViewModels.Elements;
using SightKeeper.UI.Avalonia.ViewModels.Windows;

namespace SightKeeper.UI.Avalonia.Views.Windows;

public partial class ModelEditorDialog : ReactiveWindow<ModelEditorViewModel>, Dialog<ModelEditorDialog.DialogResult>
{
	public enum DialogResult
	{
		None,
		Cancel,
		Apply
	}
	
	public ModelEditorDialog(ModelViewModel model)
	{
		InitializeComponent();
		ViewModel = ModelEditorViewModel.Create(model);
		this.WhenActivated(disposables =>
		{
			disposables(ViewModel.ApplyCommand.Subscribe(_ => Close(DialogResult.Apply)));
			disposables(ViewModel.CancelCommand.Subscribe(_ => Close(DialogResult.Cancel)));
		});
#if DEBUG
		this.AttachDevTools();
#endif
	}

	public ModelEditorDialog() : this(null!)
	{
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
	}

	public new async Task<DialogResult> ShowDialog(Window owner) =>
		await base.ShowDialog<DialogResult>(owner);
}