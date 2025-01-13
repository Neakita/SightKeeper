using System;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Detector;

namespace SightKeeper.Avalonia.Annotation.Contexts;

public sealed class DataSetAnnotationContextFactory
{
	public DataSetAnnotationContextFactory(Composition composition)
	{
		_composition = composition;
	}

	public DataSetAnnotationContext? ReuseContextOrCreateNew(
		DataSetAnnotationContext? context,
		DataSet? dataSet)
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
				if (context is not ClassifierAnnotationContext classifierContext)
				{
					if (context is IDisposable disposable)
						disposable.Dispose();
					classifierContext = _composition.ClassifierAnnotationContext;
				}
				classifierContext.DataSet = classifierDataSet;
				return classifierContext;
			case DetectorDataSet detectorDataSet:
				if (context is not DetectorAnnotationContext detectorContext)
				{
					if (context is IDisposable disposable)
						disposable.Dispose();
					detectorContext = _composition.DetectorAnnotationContext;
				}
				detectorContext.DataSet = detectorDataSet;
				return detectorContext;
			default:
				throw new ArgumentOutOfRangeException(nameof(dataSet), dataSet, null);
		}
	}

	private readonly Composition _composition;
}