using SightKeeper.Avalonia.Annotation.Drawing.Poser;
using SightKeeper.Domain.DataSets.Poser2D;
using PoserSideBarViewModel = SightKeeper.Avalonia.Annotation.Tooling.PoserSideBarViewModel;

namespace SightKeeper.Avalonia.Annotation.Contexts;

public sealed class Poser2DAnnotationContext : DataSetAnnotationContext
{
	public override PoserSideBarViewModel SideBar { get; }
	public override Poser2DDrawerViewModel Drawer { get; }
	public Poser2DDataSet? DataSet { get; set; }

	public Poser2DAnnotationContext(PoserSideBarViewModel sideBar, Poser2DDrawerViewModel drawer)
	{
		SideBar = sideBar;
		Drawer = drawer;
	}
}