using SightKeeper.Avalonia.Annotation.Drawing;

namespace SightKeeper.Avalonia.Annotation.Contexts;

public abstract class DataSetAnnotationContext : ViewModel
{
	public abstract ViewModel? SideBar { get; }
	public abstract DrawerViewModel? Drawer { get; }
}