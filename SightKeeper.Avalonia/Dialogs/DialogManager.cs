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
		get;
		private set => SetProperty(ref field, value);
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

	private void CloseDialog(Dialog dialog)
	{
		Guard.IsNotNull(CurrentDialog);
		Guard.IsReferenceEqualTo(dialog, CurrentDialog);
		if (_pendingDialogs.TryPop(out var newDialog))
			CurrentDialog = newDialog;
		else
			CurrentDialog = null;
	}
}