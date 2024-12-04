using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Domain.Model.DataSets.Poser2D;

public sealed class Poser2DDataSet : DataSet
{
	public override TagsLibrary<PoserTag> TagsLibrary { get; }
	public override AssetsLibrary<Poser2DAsset> AssetsLibrary { get; }
	public override PoserWeightsLibrary WeightsLibrary { get; }

	public Poser2DDataSet()
	{
		PoserIterativeTagsUsageProvider<Poser2DItem> tagsUsageProvider = new();
		TagsLibrary = new TagsLibrary<PoserTag>(PoserTagsFactory.Instance, tagsUsageProvider);
		Poser2DAssetsFactory assetsFactory = new(TagsLibrary);
		AssetsLibrary = new AssetsLibrary<Poser2DAsset>(assetsFactory);
		tagsUsageProvider.AssetsSource = AssetsLibrary.Assets;
		WeightsLibrary = new PoserWeightsLibrary(TagsLibrary);
	}
}