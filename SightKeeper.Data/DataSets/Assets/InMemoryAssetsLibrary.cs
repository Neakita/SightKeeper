using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets.Assets;

internal sealed class InMemoryAssetsLibrary<TAsset>(AssetFactory<TAsset> assetFactory) : AssetsOwner<TAsset> where TAsset : Asset
{
	public IReadOnlyCollection<TAsset> Assets => _assets.Values;
	public IReadOnlyCollection<Image> Images => _assets.Keys;

	public TAsset GetAsset(Image image)
	{
		return _assets[image];
	}

	public TAsset? GetOptionalAsset(Image image)
	{
		return _assets.GetValueOrDefault(image);
	}

	public bool Contains(Image image)
	{
		return _assets.ContainsKey(image);
	}

	public TAsset MakeAsset(Image image)
	{
		var asset = assetFactory.CreateAsset(image);
		_assets.Add(image, asset);
		image.AddAsset(asset);
		return asset;
	}

	public void DeleteAsset(Image image)
	{
		var asset = GetAsset(image);
		bool isRemoved = _assets.Remove(image);
		Guard.IsTrue(isRemoved);
		image.RemoveAsset(asset);
	}

	public void ClearAssets()
	{
		_assets.Clear();
	}

	internal void EnsureCapacity(int capacity)
	{
		_assets.EnsureCapacity(capacity);
	}

	private readonly Dictionary<Image, TAsset> _assets = new();
}