using CommunityToolkit.Diagnostics;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets.Assets;

internal sealed class InMemoryAssetsLibrary<TAsset>(AssetFactory<TAsset> assetFactory)
	: AssetsOwner<TAsset>, PostWrappingInitializable<DataSet<Tag, ReadOnlyAsset>>
	where TAsset : Asset
{
	public IReadOnlyCollection<TAsset> Assets => _assets.Values;
	public IReadOnlyCollection<ManagedImage> Images => _assets.Keys;

	public void Initialize(DataSet<Tag, ReadOnlyAsset> wrapped)
	{
		if (assetFactory is PostWrappingInitializable<DataSet<Tag, ReadOnlyAsset>> initializable)
			initializable.Initialize(wrapped);
	}

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