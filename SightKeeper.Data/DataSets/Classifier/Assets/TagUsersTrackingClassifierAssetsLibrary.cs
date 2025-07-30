using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.ImageSets.Images;

namespace SightKeeper.Data.DataSets.Classifier.Assets;

internal sealed class TagUsersTrackingClassifierAssetsLibrary(StorableAssetsOwner<StorableClassifierAsset> inner) : StorableAssetsOwner<StorableClassifierAsset>
{
	public IReadOnlyCollection<StorableClassifierAsset> Assets => inner.Assets;

	public IReadOnlyCollection<StorableImage> Images => inner.Images;

	public bool Contains(StorableImage image)
	{
		return inner.Contains(image);
	}

	public StorableClassifierAsset GetAsset(StorableImage image)
	{
		return inner.GetAsset(image);
	}

	public StorableClassifierAsset? GetOptionalAsset(StorableImage image)
	{
		return inner.GetOptionalAsset(image);
	}

	public void ClearAssets()
	{
		foreach (var asset in Assets)
			asset.Tag.RemoveUser(asset);
		inner.ClearAssets();
	}

	public StorableClassifierAsset MakeAsset(StorableImage image)
	{
		var asset = inner.MakeAsset(image);
		asset.Tag.AddUser(asset);
		return asset;
	}

	public void DeleteAsset(StorableImage image)
	{
		var asset = GetAsset(image);
		inner.DeleteAsset(image);
		asset.Tag.RemoveUser(asset);
	}

	public void EnsureCapacity(int capacity)
	{
		inner.EnsureCapacity(capacity);
	}
}