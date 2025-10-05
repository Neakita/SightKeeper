using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace SightKeeper.Avalonia.Dialogs;

internal abstract partial class DialogViewModel : ViewModel, Dialog
{
	public abstract string Header { get; }
	ICommand Dialog.CloseCommand => CloseCommand;

	public Task WaitAsync()
	{
		return _completionSource.Task;
	}

	[RelayCommand]
	private void Close()
	{
		_completionSource.SetResult();
	}

	private readonly TaskCompletionSource _completionSource = new();
}

internal abstract partial class DialogViewModel<TResult> : ViewModel, Dialog<TResult>
{
	public abstract string Header { get; }
	ICommand Dialog.CloseCommand => CloseCommand;

	public Task<TResult> GetResultAsync()
	{
		return _completionSource.Task;
	}
	
	protected abstract TResult DefaultResult { get; }

	protected virtual void Return(TResult result)
	{
		_completionSource.SetResult(result);
	}

	private readonly TaskCompletionSource<TResult> _completionSource = new();

	[RelayCommand]
	public void Close()
	{
		Return(DefaultResult);
	}
}