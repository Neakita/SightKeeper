using System;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Avalonia.Annotation.Tooling.Classifier;
using SightKeeper.Avalonia.Annotation.Tooling.Detector;
using SightKeeper.Avalonia.Annotation.Tooling.Poser;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Poser;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public sealed class ToolingViewModelFactory(ImagesViewModel imagesViewModel)
{
	public ViewModel? CreateToolingViewModel(DataSet? dataSet)
	{
		switch (dataSet)
		{
			case null:
				return null;
			case ClassifierDataSet classifierDataSet:
				var classifierTooling = new ClassifierToolingViewModel(imagesViewModel);
				classifierTooling.DataSet = classifierDataSet;
				return classifierTooling;
			case DetectorDataSet detectorDataSet:
				DetectorToolingViewModel detectorTooling = new()
				{
					TagsContainer = detectorDataSet.TagsLibrary
				};
				return detectorTooling;
			case PoserDataSet poserDataSet:
				var poserTooling = new PoserToolingViewModel();
				poserTooling.TagsSource = poserDataSet.TagsLibrary;
				return poserTooling;
			default:
				throw new ArgumentOutOfRangeException(nameof(dataSet));
		}
	}
}