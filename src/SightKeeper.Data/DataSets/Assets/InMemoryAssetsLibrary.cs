using CommunityToolkit.Diagnostics;
using SightKeeper.Data.ImageSets.Images;
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
		image.GetFirst<EditableImageAssets>().Add(asset);
		return asset;
	}

	public void DeleteAsset(ManagedImage image)
	{
		var asset = GetAsset(image);
		bool isRemoved = _assets.Remove(image);
		Guard.IsTrue(isRemoved);
		image.GetFirst<EditableImageAssets>().Remove(asset);
	}

	public void ClearAssets()
	{
		foreach (var (image, asset) in _assets)
			image.GetFirst<EditableImageAssets>().Remove(asset);
		_assets.Clear();
	}

	private readonly Dictionary<ManagedImage, TAsset> _assets = new();
}