using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.DataSets.Assets;

public sealed class DomainAssetsLibrary<TAsset>(AssetsOwner<TAsset> inner) : AssetsOwner<TAsset> where TAsset : Asset
{
	public IReadOnlyCollection<TAsset> Assets => inner.Assets;
	public IReadOnlyCollection<Image> Images => inner.Images;

	public TAsset MakeAsset(Image image)
	{
		if (Contains(image))
			throw new ArgumentException("Already contains asset for provided image", nameof(image));
		var asset = inner.MakeAsset(image);
		image.AddAsset(asset);
		return asset;
	}

	public void DeleteAsset(Image image)
	{
		var asset = GetAsset(image);
		inner.DeleteAsset(image);
		image.RemoveAsset(asset);
	}

	public void ClearAssets()
	{
		foreach (var (image, asset) in Images.Zip(Assets))
			image.RemoveAsset(asset);
		inner.ClearAssets();
	}

	public bool Contains(Image image)
	{
		return inner.Contains(image);
	}

	public TAsset GetAsset(Image image)
	{
		return inner.GetAsset(image);
	}

	public TAsset? GetOptionalAsset(Image image)
	{
		return inner.GetOptionalAsset(image);
	}
}