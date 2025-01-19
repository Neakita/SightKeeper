using SightKeeper.Avalonia.Annotation.Drawing.Poser;
using SightKeeper.Avalonia.Annotation.ToolBars;
using SightKeeper.Domain.DataSets.Poser3D;

namespace SightKeeper.Avalonia.Annotation.Contexts;

public sealed class Poser3DAnnotationContext : DataSetAnnotationContext
{
	public override PoserToolBarViewModel Annotation { get; }
	public override Poser3DDrawerViewModel Drawer { get; }

	public Poser3DDataSet? DataSet { get; set; }

	public Poser3DAnnotationContext(PoserToolBarViewModel annotation, Poser3DDrawerViewModel drawer)
	{
		Annotation = annotation;
		Drawer = drawer;
	}
}