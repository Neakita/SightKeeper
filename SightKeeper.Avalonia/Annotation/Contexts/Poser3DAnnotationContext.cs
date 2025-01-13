using SightKeeper.Avalonia.Annotation.Drawing.Poser;
using SightKeeper.Avalonia.Annotation.ToolBars;
using SightKeeper.Domain.DataSets.Poser3D;

namespace SightKeeper.Avalonia.Annotation.Contexts;

public sealed class Poser3DAnnotationContext : DataSetAnnotationContext
{
	public override PoserToolBarViewModel ToolBar { get; }
	public override Poser3DDrawerViewModel Drawer { get; }

	public Poser3DDataSet? DataSet { get; set; }

	public Poser3DAnnotationContext(PoserToolBarViewModel toolBar, Poser3DDrawerViewModel drawer)
	{
		ToolBar = toolBar;
		Drawer = drawer;
	}
}