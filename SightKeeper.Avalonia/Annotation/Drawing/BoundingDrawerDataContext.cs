using System.Windows.Input;

namespace SightKeeper.Avalonia.Annotation.Drawing;

public interface BoundingDrawerDataContext
{
	ICommand CreateItemCommand { get; }
}