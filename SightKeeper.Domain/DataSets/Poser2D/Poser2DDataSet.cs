using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser;

namespace SightKeeper.Domain.DataSets.Poser2D;

public interface Poser2DDataSet : PoserDataSet
{
	new AssetsOwner<Poser2DAsset> AssetsLibrary { get; }

	AssetsOwner<PoserAsset> PoserDataSet.AssetsLibrary => AssetsLibrary;
}