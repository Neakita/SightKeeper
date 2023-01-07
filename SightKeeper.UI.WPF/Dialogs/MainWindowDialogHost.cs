using System.Threading;
using System.Threading.Tasks;
using SightKeeper.UI.WPF.Views.Windows;

namespace SightKeeper.UI.WPF.Dialogs;

public sealed class MainWindowDialogHost : IDialogHost<MainWindow>
{
	public TResult ShowDialog<TResult, TDialog>(TDialog dialogContent) where TDialog : IDialog<TResult>
	{
		ShowDialogInMainWindow(dialogContent);
		return dialogContent.GetResult();
	}

	public Task<TResult> ShowDialogAsync<TResult, TDialog>(TDialog dialogContent, CancellationToken cancellationToken = default) where TDialog : IDialog<TResult>
	{
		ShowDialogInMainWindow(dialogContent);
		return dialogContent.GetResultAsync(cancellationToken);
	}


	private void ShowDialogInMainWindow<TDialog>(TDialog dialog) where TDialog : IDialog
	{
		
	}
}
