using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Input.Platform;
using Material.Icons;
using ReactiveUI;
using SightKeeper.UI.Avalonia.Extensions;
using SightKeeper.UI.Avalonia.Views.Windows;

namespace SightKeeper.UI.Avalonia.ViewModels.Windows;

public sealed class MessageBoxDialogViewModel : ViewModel
{
	public static MessageBoxDialogViewModel DesignTimeInstance => new(
		MessageBoxDialog.DialogResult.Ok | MessageBoxDialog.DialogResult.Cancel,
		"Some message",
		"Some title",
		MaterialIconKind.Abc);
	
	public IReadOnlyCollection<MessageBoxDialog.DialogResult> DialogResults { get; }
	public string Title { get; }
	public MaterialIconKind? Icon { get; }
	public string Message { get; }
	
	public ReactiveCommand<MessageBoxDialog.DialogResult, MessageBoxDialog.DialogResult> DoneCommand { get; }

	public MessageBoxDialogViewModel(MessageBoxDialog.DialogResult dialogResults, string message, string title = "", MaterialIconKind? icon = null)
	{
		DialogResults = dialogResults.GetFlags();
		Message = message;
		Title = title;
		Icon = icon;
		DoneCommand = ReactiveCommand.Create<MessageBoxDialog.DialogResult, MessageBoxDialog.DialogResult>(result => result);
	}

	private async Task CopyMessage()
	{
		global::Avalonia.Application? application = global::Avalonia.Application.Current;
		if (application == null) throw new Exception("Application is not set");
		IClipboard? applicationClipboard = application.Clipboard;
		if (applicationClipboard == null) throw new Exception("Application clipboard is not set");
		await applicationClipboard.SetTextAsync(Message);
	}
}
