using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Avalonia.Controls;
using Material.Icons;
using ReactiveUI;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Avalonia.Views.Windows;

namespace SightKeeper.Avalonia.ViewModels.Windows;

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
		var applicationClipboard = TopLevel.GetTopLevel(ServiceLocator.Instance.Resolve<MainWindow>())!.Clipboard;
		if (applicationClipboard == null) throw new Exception("Application clipboard is not set");
		await applicationClipboard.SetTextAsync(Message);
	}
}
