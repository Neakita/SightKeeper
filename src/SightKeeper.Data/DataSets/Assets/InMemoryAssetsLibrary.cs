using CommunityToolkit.Diagnostics;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Data.DataSets.Assets;

internal sealed class InMemoryAssetsLibrary<TAsset>(AssetFactory<TAsset> assetFactory) : StorableAssetsOwner<TAsset> where TAsset : Asset
{
	public IReadOnlyCollection<TAsset> Assets => _assets.Values;
	public IReadOnlyCollection<StorableImage> Images => _assets.Keys;

	public TAsset GetAsset(StorableImage image)
	{
		return _assets[image];
	}

	public TAsset? GetOptionalAsset(StorableImage image)
	{
		return _assets.GetValueOrDefault(image);
	}

	public bool Contains(StorableImage image)
	{
		return _assets.ContainsKey(image);
	}

	public TAsset MakeAsset(StorableImage image)
	{
		var asset = assetFactory.CreateAsset(image);
		_assets.Add(image, asset);
		image.AddAsset(asset);
		return asset;
	}

	public void DeleteAsset(StorableImage image)
	{
		var asset = GetAsset(image);
		bool isRemoved = _assets.Remove(image);
		Guard.IsTrue(isRemoved);
		image.RemoveAsset(asset);
	}

	public void ClearAssets()
	{
		foreach (var (image, asset) in _assets)
			image.RemoveAsset(asset);
		_assets.Clear();
	}

	public void EnsureCapacity(int capacity)
	{
		_assets.EnsureCapacity(capacity);
	}

	private readonly Dictionary<StorableImage, TAsset> _assets = new();
}