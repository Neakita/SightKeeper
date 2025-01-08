using System.Threading.Tasks;
using System.Windows.Input;

namespace SightKeeper.Avalonia.Dialogs;

public interface Dialog
{
	string Header { get; }
	ICommand CloseCommand { get; }
	Task WaitAsync();
	void Close();
}

public interface Dialog<TResult> : Dialog
{
	Task Dialog.WaitAsync() => GetResultAsync();
	Task<TResult> GetResultAsync();
}