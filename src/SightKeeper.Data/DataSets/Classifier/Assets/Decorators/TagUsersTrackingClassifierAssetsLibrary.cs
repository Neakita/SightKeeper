using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets.Classifier.Assets.Decorators;

internal sealed class TagUsersTrackingClassifierAssetsLibrary(AssetsOwner<ClassifierAsset> inner) : AssetsOwner<ClassifierAsset>
{
	public IReadOnlyCollection<ClassifierAsset> Assets => inner.Assets;

	public IReadOnlyCollection<ManagedImage> Images => inner.Images;

	public bool Contains(ManagedImage image)
	{
		return inner.Contains(image);
	}

	public ClassifierAsset GetAsset(ManagedImage image)
	{
		return inner.GetAsset(image);
	}

	public ClassifierAsset? GetOptionalAsset(ManagedImage image)
	{
		return inner.GetOptionalAsset(image);
	}

	public void ClearAssets()
	{
		foreach (var asset in Assets)
			asset.Tag.GetFirst<EditableTagUsers>().RemoveUser(asset);
		inner.ClearAssets();
	}

	public ClassifierAsset MakeAsset(ManagedImage image)
	{
		var asset = inner.MakeAsset(image);
		asset.Tag.GetFirst<EditableTagUsers>().AddUser(asset);
		return asset;
	}

	public void DeleteAsset(ManagedImage image)
	{
		var asset = GetAsset(image);
		inner.DeleteAsset(image);
		asset.Tag.GetFirst<EditableTagUsers>().RemoveUser(asset);
	}
}