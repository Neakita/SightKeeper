using SightKeeper.Avalonia.Annotation.Drawing;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public interface SelectedItemConsumer
{
	DrawerItemDataContext? SelectedItem { set; }
}