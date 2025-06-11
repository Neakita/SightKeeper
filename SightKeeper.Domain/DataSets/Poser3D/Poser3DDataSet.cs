using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser;

namespace SightKeeper.Domain.DataSets.Poser3D;

public interface Poser3DDataSet : PoserDataSet
{
	new DomainAssetsLibrary<Poser3DAsset> AssetsLibrary { get; }
	AssetsOwner<PoserAsset> PoserDataSet.AssetsLibrary => AssetsLibrary;
}