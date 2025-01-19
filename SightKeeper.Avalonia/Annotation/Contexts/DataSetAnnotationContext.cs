using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.ToolBars;

namespace SightKeeper.Avalonia.Annotation.Contexts;

public abstract class DataSetAnnotationContext : ViewModel
{
	public abstract ToolBarViewModel? Annotation { get; }
	public abstract DrawerViewModel? Drawer { get; }
}