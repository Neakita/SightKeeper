using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Poser2D;

namespace SightKeeper.Domain.DataSets.Poser3D;

public interface Poser3DDataSet : PoserDataSet
{
	new AssetsOwner<PoserAsset<DomainPoser3DItem>> AssetsLibrary { get; }
	AssetsOwner<PoserAsset<PoserItem>> PoserDataSet.AssetsLibrary => AssetsLibrary;
}