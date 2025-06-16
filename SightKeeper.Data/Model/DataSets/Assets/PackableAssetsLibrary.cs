using CommunityToolkit.Diagnostics;
using MemoryPack;
using SightKeeper.Data.Model.Images;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Data.Model.DataSets.Assets;

[MemoryPackable]
internal sealed partial class PackableAssetsLibrary<TAsset> : AssetsOwner<TAsset>
{
	[MemoryPackIgnore] public IReadOnlyCollection<TAsset> Assets => _assets.Values;
	[MemoryPackIgnore] public IReadOnlyCollection<PackableImage> Images => _assets.Keys;
	IReadOnlyCollection<DomainImage> AssetsContainer<TAsset>.Images => Images;

	public TAsset GetAsset(DomainImage image)
	{
		return _assets[(PackableImage)image];
	}

	public TAsset? GetOptionalAsset(DomainImage image)
	{
		return _assets.GetValueOrDefault((PackableImage)image);
	}

	public bool Contains(DomainImage image)
	{
		return _assets.ContainsKey((PackableImage)image);
	}

	public TAsset MakeAsset(DomainImage image)
	{
		throw new NotImplementedException();
	}

	public void DeleteAsset(DomainImage image)
	{
		var isRemoved = _assets.Remove((PackableImage)image);
		Guard.IsTrue(isRemoved);
	}

	public void ClearAssets()
	{
		_assets.Clear();
	}

	public PackableAssetsLibrary(Dictionary<PackableImage, TAsset> assets)
	{
		_assets = assets;
	}

	[MemoryPackInclude]
	private readonly Dictionary<PackableImage, TAsset> _assets;
}