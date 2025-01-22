using System;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Detector;

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
			default:
				throw new ArgumentOutOfRangeException(nameof(dataSet));
		}
	}

	private readonly Composition _composition;
}