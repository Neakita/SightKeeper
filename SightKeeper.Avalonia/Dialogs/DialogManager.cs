using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SightKeeper.Avalonia.Dialogs;

internal sealed class DialogManager : ObservableObject
{
	public Dialog? CurrentDialog
	{
		get => _currentDialog;
		private set => SetProperty(ref _currentDialog, value);
	}

	public async Task ShowDialogAsync(Dialog dialog)
	{
		if (CurrentDialog != null)
			_pendingDialogs.Push(CurrentDialog);
		CurrentDialog = dialog;
		await dialog.WaitAsync();
		CloseDialog(dialog);
	}

	public async Task<TResult> ShowDialogAsync<TResult>(Dialog<TResult> dialog)
	{
		if (CurrentDialog != null)
			_pendingDialogs.Push(CurrentDialog);
		CurrentDialog = dialog;
		var result = await dialog.GetResultAsync();
		CloseDialog(dialog);
		return result;
	}

	private readonly Stack<Dialog> _pendingDialogs = new();
	private Dialog? _currentDialog;

	private void CloseDialog(Dialog dialog)
	{
		Guard.IsNotNull(CurrentDialog);
		Guard.IsReferenceEqualTo(dialog, CurrentDialog);
		if (_pendingDialogs.Any())
			CurrentDialog = _pendingDialogs.Pop();
		else
			CurrentDialog = null;
	}
}