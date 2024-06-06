namespace SightKeeper.Domain.Model.DataSets.Classifier;

public class ClassifierAssetsLibrary : AssetsLibrary<ClassifierAsset>
{
	public ClassifierAsset MakeAsset(Screenshot screenshot, Tag tag)
	{
		ClassifierAsset asset = new(screenshot, tag);
		AddAsset(asset);
		return asset;
	}
}