using System.Threading;
using System.Threading.Tasks;

namespace SightKeeper.UI.WPF.Dialogs;

public interface IDialog
{
	string Header { get; }
}

public interface IDialog<TResult> : IDialog
{
	TResult GetResult();
	Task<TResult> GetResultAsync(CancellationToken cancellationToken = default);
}