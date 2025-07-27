using System;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Avalonia.Annotation.Tooling.Classifier;
using SightKeeper.Avalonia.Annotation.Tooling.Detector;
using SightKeeper.Avalonia.Annotation.Tooling.Poser;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Poser;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public sealed class ToolingViewModelFactory(ImageSelection imageSelection, SelectedItemProvider selectedItemProvider)
{
	public ViewModel? CreateToolingViewModel(DataSet? dataSet)
	{
		switch (dataSet)
		{
			case null:
				return null;
			case ClassifierDataSet classifierDataSet:
				var classifierTooling = new ClassifierToolingViewModel(imageSelection);
				classifierTooling.DataSet = classifierDataSet;
				return classifierTooling;
			case DetectorDataSet detectorDataSet:
				DetectorToolingViewModel detectorTooling = new()
				{
					TagsContainer = detectorDataSet.TagsLibrary
				};
				return detectorTooling;
			case PoserDataSet poserDataSet:
				var poserTooling = new PoserToolingViewModel(selectedItemProvider);
				poserTooling.TagsSource = poserDataSet.TagsLibrary;
				return poserTooling;
			default:
				throw new ArgumentOutOfRangeException(nameof(dataSet));
		}
	}
}