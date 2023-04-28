using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.ReactiveUI;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Avalonia.ViewModels.Windows;

namespace SightKeeper.Avalonia.Views.Windows;

public partial class ModelEditorDialog : ReactiveWindow<ModelEditorVM>, Dialog<ModelEditorDialog.DialogResult>
{
	public enum DialogResult
	{
		None,
		Cancel,
		Apply
	}

	public ModelEditorDialog(ModelEditorVM viewModel) : this() => ViewModel = viewModel;

	public ModelEditorDialog() => InitializeComponent();

	public new async Task<DialogResult> ShowDialog(Window owner) =>
		await base.ShowDialog<DialogResult>(owner);

	private void Cancel(object? sender, RoutedEventArgs e) => Close(DialogResult.Cancel);
	private void Apply(object? sender, RoutedEventArgs e) => Close(DialogResult.Apply);
}