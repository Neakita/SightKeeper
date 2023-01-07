using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MaterialDesignThemes.Wpf;

namespace SightKeeper.UI.WPF.Dialogs;

public sealed class GenericMDDialogHost<TWindow> : IDialogHost<TWindow> where TWindow : Window
{
	public GenericMDDialogHost(IMDDialogHostProvider<TWindow> mdDialogHostProvider) => _mdDialogHostProvider = mdDialogHostProvider;


	public TResult ShowDialog<TResult, TDialog>(TDialog dialogContent) where TDialog : IDialog<TResult>
	{
		DialogHost.ShowDialog(dialogContent);
		return dialogContent.GetResult();
	}

	public Task<TResult> ShowDialogAsync<TResult, TDialog>(TDialog dialogContent, CancellationToken cancellationToken = default) where TDialog : IDialog<TResult>
	{
		DialogHost.ShowDialog(dialogContent);
		return dialogContent.GetResultAsync(cancellationToken);
	}


	private readonly IMDDialogHostProvider<TWindow> _mdDialogHostProvider;
	private DialogHost DialogHost => _mdDialogHostProvider.DialogHost;
}