using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets.Assets;

internal sealed class InMemoryAssetsLibrary<TAsset>(AssetFactory<TAsset> assetFactory) : AssetsOwner<TAsset> where TAsset : Asset
{
	public IReadOnlyCollection<TAsset> Assets => _assets.Values;
	public IReadOnlyCollection<ManagedImage> Images => _assets.Keys;

	public TAsset GetAsset(ManagedImage image)
	{
		return _assets[image];
	}

	public TAsset? GetOptionalAsset(ManagedImage image)
	{
		return _assets.GetValueOrDefault(image);
	}

	public bool Contains(ManagedImage image)
	{
		return _assets.ContainsKey(image);
	}

	public TAsset MakeAsset(ManagedImage image)
	{
		var asset = assetFactory.CreateAsset(image);
		_assets.Add(image, asset);
		image.AddAsset(asset);
		return asset;
	}

	public void DeleteAsset(ManagedImage image)
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

	private readonly Dictionary<ManagedImage, TAsset> _assets = new();
}