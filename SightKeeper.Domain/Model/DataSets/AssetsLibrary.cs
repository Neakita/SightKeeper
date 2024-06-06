using System.Collections;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets;

public abstract class AssetsLibrary<TAsset> : IReadOnlyCollection<TAsset> where TAsset : Asset
{
	public int Count => _assets.Count;

	public IEnumerator<TAsset> GetEnumerator()
	{
		return _assets.GetEnumerator();
	}

	public void DeleteAsset(TAsset asset)
	{
		var isRemoved = _assets.Remove(asset);
		Guard.IsTrue(isRemoved);
	}

	protected void AddAsset(TAsset asset)
	{
		var isAdded = _assets.Add(asset);
		Guard.IsTrue(isAdded);
	}

	private readonly HashSet<TAsset> _assets = new();

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}