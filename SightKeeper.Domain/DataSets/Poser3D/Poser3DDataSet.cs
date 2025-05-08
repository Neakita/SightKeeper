using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.DataSets.Poser3D;

public sealed class Poser3DDataSet : PoserDataSet
{
	public override AssetsLibrary<Poser3DAsset> AssetsLibrary { get; }
	public override PoserWeightsLibrary WeightsLibrary { get; }

	public Poser3DDataSet()
	{
		Poser3DAssetsFactory assetsFactory = new(TagsLibrary);
		AssetsLibrary = new AssetsLibrary<Poser3DAsset>(assetsFactory);
		WeightsLibrary = new PoserWeightsLibrary(TagsLibrary);
	}
}