using System;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

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
				TagSelectionViewModel<Tag> tagSelection = new()
				{
					Tags = detectorDataSet.TagsLibrary.Tags
				};
				return tagSelection;
			case PoserDataSet poserDataSet:
				var poserTooling = _composition.PoserToolingViewModel;
				poserTooling.TagsSource = poserDataSet.TagsLibrary;
				return poserTooling;
			default:
				throw new ArgumentOutOfRangeException(nameof(dataSet));
		}
	}

	private readonly Composition _composition;
}