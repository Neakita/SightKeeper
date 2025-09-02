using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Avalonia.Annotation.Tooling;

namespace SightKeeper.Avalonia.Annotation;

public interface AnnotationTabDataContext
{
	ImagesDataContext Images { get; }
	DrawerDataContext Drawer { get; }
	SideBarDataContext SideBar { get; }
}