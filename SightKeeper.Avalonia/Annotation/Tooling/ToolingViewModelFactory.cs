using System;
using SightKeeper.Avalonia.Annotation.Tooling.Detector;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Poser;

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
			case DomainClassifierDataSet classifierDataSet:
				var classifierTooling = _composition.ClassifierToolingViewModel;
				classifierTooling.DataSet = classifierDataSet;
				return classifierTooling;
			case DomainDetectorDataSet detectorDataSet:
				DetectorToolingViewModel detectorTooling = new()
				{
					TagsContainer = detectorDataSet.TagsLibrary
				};
				return detectorTooling;
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