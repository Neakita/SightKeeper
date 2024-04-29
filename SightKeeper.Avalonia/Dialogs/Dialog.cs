using System.Threading.Tasks;
using System.Windows.Input;

namespace SightKeeper.Avalonia.Dialogs;

internal interface Dialog
{
	string Header { get; }
	ICommand CloseCommand { get; }
	Task WaitAsync();
	void Close();
}
internal interface Dialog<TResult> : Dialog
{
	Task Dialog.WaitAsync() => GetResultAsync();
	Task<TResult> GetResultAsync();
}