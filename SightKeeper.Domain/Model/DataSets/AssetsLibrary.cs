using System.Collections;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets;

public abstract class AssetsLibrary : IReadOnlyCollection<Asset>
{
	public abstract int Count { get; }
	public abstract DataSet DataSet { get; }

	public abstract IEnumerator<Asset> GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}

public sealed class AssetsLibrary<TAsset> : AssetsLibrary, IReadOnlyCollection<TAsset>
	where TAsset : Asset, AssetsFactory<TAsset>, AssetsDestroyer<TAsset>
{
	public override int Count => _assets.Count;
	public override DataSet DataSet { get; }

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

	public override IEnumerator<TAsset> GetEnumerator()
	{
		return _assets.GetEnumerator();
	}

	private void AddAsset(TAsset asset)
	{
		var isAdded = _assets.Add(asset);
		Guard.IsTrue(isAdded);
	}

	private readonly HashSet<TAsset> _assets = new();
}