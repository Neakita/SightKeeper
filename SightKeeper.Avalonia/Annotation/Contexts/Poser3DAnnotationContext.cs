using System;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.ToolBars;
using SightKeeper.Domain.DataSets.Poser3D;

namespace SightKeeper.Avalonia.Annotation.Contexts;

public sealed class Poser3DAnnotationContext : DataSetAnnotationContext
{
	public override ToolBarViewModel ToolBar => throw new NotImplementedException();
	public override DrawerViewModel Drawer => throw new NotImplementedException();
	
	public Poser3DDataSet? DataSet { get; set; }
}