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

public abstract class AssetsLibrary<TAsset> : AssetsLibrary, IReadOnlyCollection<TAsset> where TAsset : Asset
{
	public override int Count => _assets.Count;

	public override IEnumerator<TAsset> GetEnumerator()
	{
		return _assets.GetEnumerator();
	}

	public virtual void DeleteAsset(TAsset asset)
	{
		Guard.IsTrue(_assets.Remove(asset));
	}

	protected void AddAsset(TAsset asset)
	{
		var isAdded = _assets.Add(asset);
		Guard.IsTrue(isAdded);
	}

	private readonly HashSet<TAsset> _assets = new();
}