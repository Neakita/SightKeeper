using System;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.ToolBars;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Detector;

namespace SightKeeper.Avalonia.Annotation.DataSetContexts;

internal abstract class DataSetAnnotationContextViewModel : ViewModel
{
	public static DataSetAnnotationContextViewModel? ReuseContextOrCreateNew(
		DataSetAnnotationContextViewModel? context,
		DataSet? dataSet,
		Composition composition)
	{
		switch (dataSet)
		{
			case null:
			{
				if (context is IDisposable disposable)
					disposable.Dispose();
				return null;
			}
			case ClassifierDataSet classifierDataSet:
				if (context is not ClassifierAnnotationContextViewModel classifierContext)
				{
					if (context is IDisposable disposable)
						disposable.Dispose();
					classifierContext = composition.ClassifierAnnotationContextViewModel;
				}
				classifierContext.DataSet = classifierDataSet;
				return classifierContext;
			case DetectorDataSet detectorDataSet:
				if (context is not DetectorAnnotationContextViewModel detectorContext)
				{
					if (context is IDisposable disposable)
						disposable.Dispose();
					detectorContext = composition.DetectorAnnotationContextViewModel;
				}
				detectorContext.DataSet = detectorDataSet;
				return detectorContext;
			default:
				throw new ArgumentOutOfRangeException(nameof(dataSet), dataSet, null);
		}
	}

	public abstract ToolBarViewModel? ToolBar { get; }
	public abstract DrawerViewModel? Drawer { get; }
}