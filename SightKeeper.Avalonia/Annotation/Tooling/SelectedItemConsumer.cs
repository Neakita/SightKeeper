using SightKeeper.Avalonia.Annotation.Drawing;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public interface SelectedItemConsumer
{
	BoundedItemDataContext? SelectedItem { set; }
}