using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Assets.Items;
using SightKeeper.Data.DataSets.Detector.Items;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Detector;
using Tag = SightKeeper.Domain.DataSets.Tags.Tag;

namespace SightKeeper.Data.DataSets.Detector.Decorators;

internal sealed class StorableDetectorDataSetExtension(DetectorDataSet inner, StorableDetectorDataSet innerExtended) : StorableDetectorDataSet
{
	public string Name
	{
		get => innerExtended.Name;
		set => innerExtended.Name = value;
	}

	public string Description
	{
		get => innerExtended.Description;
		set => innerExtended.Description = value;
	}

	public StorableTagsOwner<StorableTag> TagsLibrary { get; } =
		new StorableTagsOwnerExtension<Tag, StorableTag>(inner.TagsLibrary, innerExtended.TagsLibrary);

	public StorableAssetsOwner<StorableItemsAsset<StorableDetectorItem>> AssetsLibrary { get; } =
		new StorableAssetsOwnerExtension<ItemsAsset<DetectorItem>, StorableItemsAsset<StorableDetectorItem>>(inner.AssetsLibrary, innerExtended.AssetsLibrary);

	public StorableWeightsLibrary WeightsLibrary { get; } =
		new StorableWeightsLibraryExtension(inner.WeightsLibrary, innerExtended.WeightsLibrary);

	public StorableDetectorDataSet Innermost => innerExtended.Innermost;
}