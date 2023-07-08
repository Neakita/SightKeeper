using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Material.Icons;
using ReactiveUI;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Avalonia.ViewModels.Windows;

namespace SightKeeper.Avalonia.Views.Windows;

public partial class MessageBoxDialog : ReactiveWindow<MessageBoxDialogViewModel>, Dialog<MessageBoxDialog.DialogResult>
{
	public static void Show(
		string message,
		DialogResult dialogResults = DialogResult.Ok,
		string title = "",
		MaterialIconKind? icon = null)
	{
		new MessageBoxDialog(message, dialogResults, title, icon).Show();
	}
	
	[Flags]
	public enum DialogResult
	{
		None,
		Ok = 1 << 0,
		Yes = 1 << 1,
		No = 1 << 2,
		Apply = 1 << 3,
		Cancel = 1 << 4
	}
	
	public MessageBoxDialog(MessageBoxDialogViewModel viewModel)
	{
		InitializeComponent();
		ViewModel = viewModel;
		this.WhenActivated(disposables => disposables(ViewModel.DoneCommand.Subscribe(result => Close(result))));
	}

	public MessageBoxDialog(string message, DialogResult dialogResults = DialogResult.Ok, string title = "", MaterialIconKind? icon = null) : this(new MessageBoxDialogViewModel(dialogResults, message, title, icon))
	{
	}

	public MessageBoxDialog() : this("")
	{
	}

	public new Task<DialogResult> ShowDialog(Window owner) => base.ShowDialog<DialogResult>(owner);
}