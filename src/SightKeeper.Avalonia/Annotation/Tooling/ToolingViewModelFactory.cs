using System;
using SightKeeper.Avalonia.Annotation.Tooling.Classifier;
using SightKeeper.Avalonia.Annotation.Tooling.Detector;
using SightKeeper.Avalonia.Annotation.Tooling.Poser;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Tooling;

internal sealed class ToolingViewModelFactory(
	Func<DataSet<Tag, ClassifierAsset>, ClassifierToolingViewModel> classifierToolingFactory,
	Func<TagsContainer<Tag>, DetectorToolingViewModel> detectorToolingFactory,
	Func<TagsContainer<PoserTag>, PoserToolingViewModel> poserToolingFactory)
{
	public ViewModel? CreateToolingViewModel(DataSet<Tag, Asset>? dataSet)
	{
		return dataSet switch
		{
			null => null,
			DataSet<Tag, ClassifierAsset> classifierDataSet => classifierToolingFactory(classifierDataSet),
			DataSet<PoserTag, ItemsAsset<PoserItem>> poserDataSet => poserToolingFactory(poserDataSet.TagsLibrary),
			DataSet<Tag, ItemsAsset<DetectorItem>> detectorDataSet => detectorToolingFactory(detectorDataSet.TagsLibrary),
			_ => throw new ArgumentOutOfRangeException(nameof(dataSet))
		};
	}
}