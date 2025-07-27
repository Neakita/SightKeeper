using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Avalonia.Annotation.Tooling;

namespace SightKeeper.Avalonia.Annotation;

public sealed class AnnotationTabViewModel : ViewModel, AnnotationTabDataContext
{
	public required ImagesDataContext Images { get; init; }
	public required DrawerDataContext Drawer { get; init; }
	public required SideBarDataContext SideBar { get; init; }
}