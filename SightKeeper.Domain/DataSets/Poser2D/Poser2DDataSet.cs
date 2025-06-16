using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser;

namespace SightKeeper.Domain.DataSets.Poser2D;

public interface Poser2DDataSet : PoserDataSet
{
	new AssetsOwner<PoserAsset<Poser2DItem>> AssetsLibrary { get; }

	AssetsOwner<PoserAsset<PoserItem>> PoserDataSet.AssetsLibrary => AssetsLibrary;
}