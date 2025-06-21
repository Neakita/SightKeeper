using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Model.DataSets.Assets;

public sealed class StorableClassifierAssetFactory : AssetFactory<ClassifierAsset>
{
	public ClassifierAsset CreateAsset(Image image)
	{
		throw new NotImplementedException();
	}
}