using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Material.Icons;
using ReactiveUI;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Avalonia.ViewModels.Windows;

namespace SightKeeper.Avalonia.Views.Windows;

public partial class MessageBoxDialog : ReactiveWindow<MessageBoxDialogVM>, Dialog<MessageBoxDialog.DialogResult>
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
	
	public MessageBoxDialog(MessageBoxDialogVM vm)
	{
		InitializeComponent();
#if DEBUG
		this.AttachDevTools();
#endif
		ViewModel = vm;
		this.WhenActivated(disposables => disposables(ViewModel.DoneCommand.Subscribe(result => Close(result))));
	}

	public MessageBoxDialog(string message, DialogResult dialogResults = DialogResult.Ok, string title = "", MaterialIconKind? icon = null) : this(new MessageBoxDialogVM(dialogResults, message, title, icon))
	{
	}

	public MessageBoxDialog() : this("")
	{
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
	}

	public new Task<DialogResult> ShowDialog(Window owner) => base.ShowDialog<DialogResult>(owner);
}