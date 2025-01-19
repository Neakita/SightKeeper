using SightKeeper.Avalonia.Annotation.Drawing.Poser;
using SightKeeper.Avalonia.Annotation.ToolBars;
using SightKeeper.Domain.DataSets.Poser2D;

namespace SightKeeper.Avalonia.Annotation.Contexts;

public sealed class Poser2DAnnotationContext : DataSetAnnotationContext
{
	public override PoserToolBarViewModel Annotation { get; }
	public override Poser2DDrawerViewModel Drawer { get; }
	public Poser2DDataSet? DataSet { get; set; }

	public Poser2DAnnotationContext(PoserToolBarViewModel annotation, Poser2DDrawerViewModel drawer)
	{
		Annotation = annotation;
		Drawer = drawer;
	}
}