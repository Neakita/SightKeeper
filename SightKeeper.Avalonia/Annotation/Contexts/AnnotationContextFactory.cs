using System;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Poser2D;
using SightKeeper.Domain.DataSets.Poser3D;

namespace SightKeeper.Avalonia.Annotation.Contexts;

public sealed class AnnotationContextFactory
{
	public AnnotationContextFactory(Composition composition)
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
			case Poser2DDataSet poser2DDataSet:
				if (context is not Poser2DAnnotationContext poser2DContext)
				{
					if (context is IDisposable disposable)
						disposable.Dispose();
					poser2DContext = _composition.Poser2DAnnotationContext;
				}
				poser2DContext.DataSet = poser2DDataSet;
				return poser2DContext;
			case Poser3DDataSet poser3DDataSet:
				if (context is not Poser3DAnnotationContext poser3DContext)
				{
					if (context is IDisposable disposable)
						disposable.Dispose();
					poser3DContext = _composition.Poser3DAnnotationContext;
				}
				poser3DContext.DataSet = poser3DDataSet;
				return poser3DContext;
			default:
				throw new ArgumentOutOfRangeException(nameof(dataSet), dataSet, null);
		}
	}

	private readonly Composition _composition;
}