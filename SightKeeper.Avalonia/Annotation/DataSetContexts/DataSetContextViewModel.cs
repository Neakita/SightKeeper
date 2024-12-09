using System;
using Autofac;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.ToolBars;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Detector;

namespace SightKeeper.Avalonia.Annotation.DataSetContexts;

internal abstract class DataSetContextViewModel : ViewModel
{
	public static DataSetContextViewModel? Create(DataSet? dataSet, IComponentContext context) => dataSet switch
	{
		null => null,
		ClassifierDataSet classifier => context.Resolve<ClassifierContextViewModel>(
			new TypedParameter(typeof(ClassifierDataSet), classifier)),
		DetectorDataSet detector => context.Resolve<DetectorContextViewModel>(
			new TypedParameter(typeof(DetectorDataSet), detector)),
		_ => throw new ArgumentOutOfRangeException(nameof(dataSet), dataSet, null)
	};

	public abstract ToolBarViewModel? ToolBar { get; }
	public abstract DrawerViewModel? Drawer { get; }
}