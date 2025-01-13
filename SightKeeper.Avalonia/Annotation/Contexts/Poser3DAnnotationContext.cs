using System;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.ToolBars;
using SightKeeper.Domain.DataSets.Poser3D;

namespace SightKeeper.Avalonia.Annotation.Contexts;

public sealed class Poser3DAnnotationContext : DataSetAnnotationContext
{
	public override PoserToolBarViewModel ToolBar { get; }
	public override DrawerViewModel Drawer => throw new NotImplementedException();

	public Poser3DDataSet? DataSet { get; set; }

	public Poser3DAnnotationContext(PoserToolBarViewModel toolBar)
	{
		ToolBar = toolBar;
	}
}