using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.DataSets.Poser3D;

public sealed class Poser3DDataSet : PoserDataSet
{
	public override TagsLibrary<PoserTag> TagsLibrary { get; }
	public override AssetsLibrary<Poser3DAsset> AssetsLibrary { get; }
	public override PoserWeightsLibrary WeightsLibrary { get; }

	public Poser3DDataSet()
	{
		PoserIterativeTagsUsageProvider<Poser3DItem> tagsUsageProvider = new();
		PoserTagsFactory tagsFactory = new(tagsUsageProvider);
		TagsLibrary = new TagsLibrary<PoserTag>(tagsFactory, tagsUsageProvider);
		Poser3DAssetsFactory assetsFactory = new(TagsLibrary);
		AssetsLibrary = new AssetsLibrary<Poser3DAsset>(assetsFactory);
		WeightsLibrary = new PoserWeightsLibrary(TagsLibrary);
	}
}