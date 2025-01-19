using SightKeeper.Avalonia.Annotation.Drawing.Poser;
using SightKeeper.Avalonia.Annotation.SideBars;
using SightKeeper.Domain.DataSets.Poser3D;

namespace SightKeeper.Avalonia.Annotation.Contexts;

public sealed class Poser3DAnnotationContext : DataSetAnnotationContext
{
	public override PoserSideBarViewModel SideBar { get; }
	public override Poser3DDrawerViewModel Drawer { get; }

	public Poser3DDataSet? DataSet { get; set; }

	public Poser3DAnnotationContext(PoserSideBarViewModel sideBar, Poser3DDrawerViewModel drawer)
	{
		SideBar = sideBar;
		Drawer = drawer;
	}
}