using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SightKeeper.UI.WPF.Dialogs;

public interface IDialogHost<TWindow> where TWindow : Window
{
	public TResult ShowDialog<TResult, TDialog>(TDialog dialogContent) where TDialog : IDialog<TResult>;
	public Task<TResult> ShowDialogAsync<TResult, TDialog>(TDialog dialogContent, CancellationToken cancellationToken = default) where TDialog : IDialog<TResult>;
}