using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets;

internal sealed class InMemoryDataSet<TTag, TAsset>(
	TagFactory<TTag> tagFactory,
	AssetFactory<TAsset> assetFactory,
	WeightsWrapper weightsWrapper)
	: DataSet<TTag, TAsset>
	where TAsset : Asset
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public TagsOwner<TTag> TagsLibrary { get; } = new InMemoryTagsLibrary<TTag>(tagFactory);
	public AssetsOwner<TAsset> AssetsLibrary { get; } = new InMemoryAssetsLibrary<TAsset>(assetFactory);
	public WeightsLibrary WeightsLibrary { get; } = new InMemoryWeightsLibrary(weightsWrapper);
}