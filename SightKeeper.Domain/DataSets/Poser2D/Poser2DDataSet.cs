using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser;

namespace SightKeeper.Domain.DataSets.Poser2D;

public sealed class Poser2DDataSet : PoserDataSet
{
	public override DomainAssetsLibrary<Poser2DAsset> AssetsLibrary { get; }

	public Poser2DDataSet()
	{
		Poser2DAssetsFactory assetsFactory = new(TagsLibrary);
		AssetsLibrary = new DomainAssetsLibrary<Poser2DAsset>(assetsFactory);
	}
}