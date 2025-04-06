using SightKeeper.Avalonia.Annotation.Drawing;

namespace SightKeeper.Avalonia.Annotation.Tooling.Poser;

public interface SelectedItemConsumer
{
	BoundedItemDataContext? SelectedItem { set; }
}