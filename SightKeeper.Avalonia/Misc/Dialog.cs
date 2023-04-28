using System.Threading.Tasks;
using Avalonia.Controls;

namespace SightKeeper.Avalonia.Misc;

public interface Dialog<TResult>
{
	Task<TResult> ShowDialog(Window owner);
}