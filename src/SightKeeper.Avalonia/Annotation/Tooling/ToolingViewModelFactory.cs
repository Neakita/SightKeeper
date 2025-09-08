using System;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Avalonia.Annotation.Tooling.Classifier;
using SightKeeper.Avalonia.Annotation.Tooling.Detector;
using SightKeeper.Avalonia.Annotation.Tooling.Poser;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public sealed class ToolingViewModelFactory(ImageSelection imageSelection, SelectedItemProvider selectedItemProvider)
{
	public ViewModel? CreateToolingViewModel(DataSet<Tag, Asset>? dataSet)
	{
		switch (dataSet)
		{
			case null:
				return null;
			case DataSet<Tag, ClassifierAsset> classifierDataSet:
				var classifierTooling = new ClassifierToolingViewModel(imageSelection);
				classifierTooling.DataSet = classifierDataSet;
				return classifierTooling;
			case DataSet<PoserTag, ItemsAsset<PoserItem>> poserDataSet:
				var poserTooling = new PoserToolingViewModel(selectedItemProvider);
				poserTooling.TagsSource = poserDataSet.TagsLibrary;
				return poserTooling;
			case DataSet<Tag, ItemsAsset<DetectorItem>> detectorDataSet:
				DetectorToolingViewModel detectorTooling = new()
				{
					TagsContainer = detectorDataSet.TagsLibrary
				};
				return detectorTooling;
			default:
				throw new ArgumentOutOfRangeException(nameof(dataSet));
		}
	}
}