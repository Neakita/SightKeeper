using SightKeeper.Avalonia.Annotation.Drawing.Bounded;

namespace SightKeeper.Avalonia.Annotation.Tooling.Poser;

public interface SelectedItemConsumer
{
	BoundedItemDataContext? SelectedItem { set; }
}