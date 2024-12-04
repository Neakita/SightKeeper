using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Domain.Model.DataSets.Poser3D;

public sealed class Poser3DDataSet : DataSet
{
	public override TagsLibrary<PoserTag> TagsLibrary { get; }
	public override AssetsLibrary<Poser3DAsset> AssetsLibrary { get; }
	public override PoserWeightsLibrary WeightsLibrary { get; }

	public Poser3DDataSet()
	{
		PoserIterativeTagsUsageProvider<Poser3DItem> tagsUsageProvider = new();
		TagsLibrary = new TagsLibrary<PoserTag>(PoserTagsFactory.Instance, tagsUsageProvider);
		Poser3DAssetsFactory assetsFactory = new(TagsLibrary);
		AssetsLibrary = new AssetsLibrary<Poser3DAsset>(assetsFactory);
		WeightsLibrary = new PoserWeightsLibrary(TagsLibrary);
	}
}