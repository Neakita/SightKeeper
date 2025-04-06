using System.Windows.Input;

namespace SightKeeper.Avalonia.Annotation.Drawing.Bounded;

public interface BoundingDrawerDataContext
{
	ICommand CreateItemCommand { get; }
}