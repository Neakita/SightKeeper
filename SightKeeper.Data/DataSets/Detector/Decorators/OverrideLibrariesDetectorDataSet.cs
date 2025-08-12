using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Assets.Items;
using SightKeeper.Data.DataSets.Detector.Items;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Detector.Decorators;

internal sealed class OverrideLibrariesDetectorDataSet(StorableDetectorDataSet inner) : StorableDetectorDataSet
{
	public string Name
	{
		get => inner.Name;
		set => inner.Name = value;
	}

	public string Description
	{
		get => inner.Description;
		set => inner.Description = value;
	}

	public StorableTagsOwner<StorableTag> TagsLibrary { get; init; } = inner.TagsLibrary;

	public StorableAssetsOwner<StorableItemsAsset<StorableDetectorItem>> AssetsLibrary { get; init; } = inner.AssetsLibrary;

	public StorableWeightsLibrary WeightsLibrary { get; init; } = inner.WeightsLibrary;

	public StorableDetectorDataSet Innermost => inner.Innermost;
}