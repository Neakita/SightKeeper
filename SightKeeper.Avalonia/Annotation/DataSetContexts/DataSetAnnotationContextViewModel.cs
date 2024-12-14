using System;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.ToolBars;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Avalonia.Annotation.DataSetContexts;

internal abstract class DataSetAnnotationContextViewModel : ViewModel
{
	public static DataSetAnnotationContextViewModel? Create(DataSet? dataSet, Composition composition) => dataSet switch
	{
		null => null,
		/*ClassifierDataSet classifier => composition.,
		DetectorDataSet detector => context.Resolve<DetectorContextViewModel>(
			new TypedParameter(typeof(DetectorDataSet), detector)),*/
		_ => throw new ArgumentOutOfRangeException(nameof(dataSet), dataSet, null)
	};

	public abstract ToolBarViewModel? ToolBar { get; }
	public abstract DrawerViewModel? Drawer { get; }
}