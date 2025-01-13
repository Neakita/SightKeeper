using System;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.ToolBars;
using SightKeeper.Domain.DataSets.Poser2D;

namespace SightKeeper.Avalonia.Annotation.Contexts;

public sealed class Poser2DAnnotationContext : DataSetAnnotationContext
{
	public override PoserToolBarViewModel ToolBar { get; }
	public override DrawerViewModel Drawer => throw new NotImplementedException();

	public Poser2DDataSet? DataSet { get; set; }

	public Poser2DAnnotationContext(PoserToolBarViewModel toolBar)
	{
		ToolBar = toolBar;
	}
}