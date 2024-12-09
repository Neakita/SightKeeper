using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.DataSets.Poser2D;

public sealed class Poser2DDataSet : DataSet
{
	public override TagsLibrary<PoserTag> TagsLibrary { get; }
	public override AssetsLibrary<Poser2DAsset> AssetsLibrary { get; }
	public override PoserWeightsLibrary WeightsLibrary { get; }

	public Poser2DDataSet()
	{
		PoserIterativeTagsUsageProvider<Poser2DItem> tagsUsageProvider = new();
		PoserTagsFactory tagsFactory = new(tagsUsageProvider);
		TagsLibrary = new TagsLibrary<PoserTag>(tagsFactory, tagsUsageProvider);
		Poser2DAssetsFactory assetsFactory = new(TagsLibrary);
		AssetsLibrary = new AssetsLibrary<Poser2DAsset>(assetsFactory);
		tagsUsageProvider.AssetsSource = AssetsLibrary.Assets.Values;
		WeightsLibrary = new PoserWeightsLibrary(TagsLibrary);
	}
}