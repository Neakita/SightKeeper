using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Domain.Model.DataSets.Assets;

public abstract class AssetsLibrary
{
	public abstract DataSet DataSet { get; }
	public abstract IReadOnlyCollection<Asset> Assets { get; }
}

public abstract class AssetsLibrary<TAsset> : AssetsLibrary where TAsset : Asset
{
	public abstract override DataSet<TAsset> DataSet { get; }
	public override IReadOnlyCollection<TAsset> Assets => _assets;

	public TAsset MakeAsset(Screenshot<TAsset> screenshot)
	{
		Guard.IsNull(screenshot.Asset);
		var asset = CreateAsset(screenshot);
		var isAdded = _assets.Add(asset);
		Guard.IsTrue(isAdded);
		screenshot.SetAsset(asset);
		return asset;
	}

	public virtual void DeleteAsset(TAsset asset)
	{
		var isRemoved = _assets.Remove(asset);
		Guard.IsTrue(isRemoved);
	}

	protected abstract TAsset CreateAsset(Screenshot<TAsset> screenshot);

	private readonly HashSet<TAsset> _assets = new();
}