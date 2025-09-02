using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SightKeeper.Avalonia.Dialogs;

public sealed partial class DialogManager : ObservableObject
{
    [ObservableProperty]
    public partial Dialog? CurrentDialog { get; private set; }

    public async Task ShowDialogAsync(Dialog dialog)
	{
		if (CurrentDialog != null)
			_pendingDialogs.Push(CurrentDialog);
		CurrentDialog = dialog;
		await dialog.WaitAsync();
		RemoveDialog(dialog);
	}

	public async Task<TResult> ShowDialogAsync<TResult>(Dialog<TResult> dialog)
	{
		if (CurrentDialog != null)
			_pendingDialogs.Push(CurrentDialog);
		CurrentDialog = dialog;
		var result = await dialog.GetResultAsync();
		RemoveDialog(dialog);
		return result;
	}

	private readonly Stack<Dialog> _pendingDialogs = new();

	[RelayCommand]
	private void CloseCurrentDialog()
	{
		CurrentDialog?.CloseCommand.Execute(null);
	}

	private void RemoveDialog(Dialog dialog)
	{
		Guard.IsNotNull(CurrentDialog);
		Guard.IsReferenceEqualTo(dialog, CurrentDialog);
		RemoveCurrentDialog();
	}

	private void RemoveCurrentDialog()
	{
		if (_pendingDialogs.TryPop(out var newDialog))
			CurrentDialog = newDialog;
		else
			CurrentDialog = null;
	}
}