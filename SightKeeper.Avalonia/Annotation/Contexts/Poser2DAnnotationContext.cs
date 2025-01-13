using System;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.ToolBars;
using SightKeeper.Domain.DataSets.Poser2D;

namespace SightKeeper.Avalonia.Annotation.Contexts;

public sealed class Poser2DAnnotationContext : DataSetAnnotationContext
{
	public override ToolBarViewModel ToolBar => throw new NotImplementedException();
	public override DrawerViewModel Drawer => throw new NotImplementedException();

	public Poser2DDataSet? DataSet { get; set; }
}