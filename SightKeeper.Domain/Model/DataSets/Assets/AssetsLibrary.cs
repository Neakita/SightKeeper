using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Domain.Model.DataSets.Assets;

public abstract class AssetsLibrary
{
	public abstract DataSet DataSet { get; }
	public abstract IReadOnlyCollection<Asset> Assets { get; }
}

public sealed class AssetsLibrary<TAsset> : AssetsLibrary
	where TAsset : Asset, AssetsFactory<TAsset>, AssetsDestroyer<TAsset>
{
	public override DataSet DataSet { get; }
	public override IReadOnlyCollection<TAsset> Assets => _assets;

	public AssetsLibrary(DataSet dataSet)
	{
		DataSet = dataSet;
	}

	public TAsset MakeAsset(Screenshot<TAsset> screenshot)
	{
		Guard.IsNull(screenshot.Asset);
		var asset = TAsset.Create(screenshot);
		Guard.IsTrue(_assets.Add(asset));
		screenshot.SetAsset(asset);
		return asset;
	}

	public void DeleteAsset(TAsset asset)
	{
		Guard.IsTrue(_assets.Remove(asset));
		TAsset.Destroy(asset);
	}

	private readonly HashSet<TAsset> _assets = new();
}