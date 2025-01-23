using System;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Poser2D;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public sealed class ToolingViewModelFactory
{
	public ToolingViewModelFactory(Composition composition)
	{
		_composition = composition;
	}

	public ViewModel? CreateToolingViewModel(DataSet? dataSet)
	{
		switch (dataSet)
		{
			case null:
				return null;
			case ClassifierDataSet classifierDataSet:
				var classifierAnnotation = _composition.ClassifierAnnotationViewModel;
				classifierAnnotation.DataSet = classifierDataSet;
				return classifierAnnotation;
			case DetectorDataSet detectorDataSet:
				var tagSelection = _composition.TagSelectionViewModel;
				tagSelection.Tags = detectorDataSet.TagsLibrary.Tags;
				return tagSelection;
			case Poser2DDataSet poser2DDataSet:
				var poserTooling = _composition.PoserToolingViewModel;
				poserTooling.TagSelection.Tags = poser2DDataSet.TagsLibrary.Tags;
				return poserTooling;
			default:
				throw new ArgumentOutOfRangeException(nameof(dataSet));
		}
	}

	private readonly Composition _composition;
}