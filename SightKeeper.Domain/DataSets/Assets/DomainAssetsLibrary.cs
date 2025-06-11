using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.DataSets.Assets;

public sealed class DomainAssetsLibrary<TAsset> : AssetsOwner<TAsset>, Decorator<AssetsOwner<TAsset>> where TAsset : Asset
{
	public IReadOnlyCollection<TAsset> Assets => Inner.Assets;
	public IReadOnlyCollection<Image> Images => Inner.Images;
	public AssetsOwner<TAsset> Inner { get; }

	public TAsset MakeAsset(Image image)
	{
		var asset = Inner.MakeAsset(image);
		image.AddAsset(asset);
		return asset;
	}

	public void DeleteAsset(Image image)
	{
		var asset = GetAsset(image);
		Inner.DeleteAsset(image);
		image.RemoveAsset(asset);
	}

	public void ClearAssets()
	{
		foreach (var (image, asset) in Images.Zip(Assets))
			image.RemoveAsset(asset);
		Inner.ClearAssets();
	}

	public bool Contains(Image image)
	{
		return Inner.Contains(image);
	}

	public TAsset GetAsset(Image image)
	{
		return Inner.GetAsset(image);
	}

	public TAsset? GetOptionalAsset(Image image)
	{
		return Inner.GetOptionalAsset(image);
	}

	internal DomainAssetsLibrary(AssetsOwner<TAsset> inner)
	{
		Inner = inner;
	}
}