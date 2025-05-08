using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.DataSets.Poser2D;

public sealed class Poser2DDataSet : PoserDataSet
{
	public override AssetsLibrary<Poser2DAsset> AssetsLibrary { get; }
	public override PoserWeightsLibrary WeightsLibrary { get; }

	public Poser2DDataSet()
	{
		Poser2DAssetsFactory assetsFactory = new(TagsLibrary);
		AssetsLibrary = new AssetsLibrary<Poser2DAsset>(assetsFactory);
		WeightsLibrary = new PoserWeightsLibrary(TagsLibrary);
	}
}