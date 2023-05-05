using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input.Platform;
using Material.Icons;
using ReactiveUI;
using SightKeeper.Avalonia.Views.Windows;
using SightKeeper.Common;
using SightKeeper.Avalonia.Extensions;

namespace SightKeeper.Avalonia.ViewModels.Windows;

public sealed class MessageBoxDialogVM : ViewModel
{
	public static MessageBoxDialogVM DesignTimeInstance => new(
		MessageBoxDialog.DialogResult.Ok | MessageBoxDialog.DialogResult.Cancel,
		"Some message",
		"Some title",
		MaterialIconKind.Abc);
	
	public IReadOnlyCollection<MessageBoxDialog.DialogResult> DialogResults { get; }
	public string Title { get; }
	public MaterialIconKind? Icon { get; }
	public string Message { get; }
	
	public ReactiveCommand<MessageBoxDialog.DialogResult, MessageBoxDialog.DialogResult> DoneCommand { get; }

	public MessageBoxDialogVM(MessageBoxDialog.DialogResult dialogResults, string message, string title = "", MaterialIconKind? icon = null)
	{
		DialogResults = dialogResults.GetFlags();
		Message = message;
		Title = title;
		Icon = icon;
		DoneCommand = ReactiveCommand.Create<MessageBoxDialog.DialogResult, MessageBoxDialog.DialogResult>(result => result);
	}

	private async Task CopyMessage()
	{
		IClipboard? applicationClipboard = TopLevel.GetTopLevel(Locator.Resolve<MainWindow>())!.Clipboard;
		if (applicationClipboard == null) throw new Exception("Application clipboard is not set");
		await applicationClipboard.SetTextAsync(Message);
	}
}
