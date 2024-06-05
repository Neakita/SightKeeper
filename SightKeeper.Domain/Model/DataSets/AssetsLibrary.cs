using System.Collections;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets;

public sealed class AssetsLibrary : IReadOnlyCollection<Asset>
{
	public int Count => _assets.Count;

	public IEnumerator<Asset> GetEnumerator()
	{
		return _assets.GetEnumerator();
	}

	public Asset MakeAsset(Screenshot screenshot)
	{
		// TODO
		/*Guard.IsNull(screenshot.Asset);
		screenshot.Asset = new Asset(screenshot);*/
		Asset asset = new(screenshot);
		bool isAdded = _assets.Add(asset);
		Guard.IsTrue(isAdded);
		return asset;
	}

	public void DeleteAsset(Asset asset)
	{
		// TODO
		var isRemoved = _assets.Remove(asset);
		Guard.IsTrue(isRemoved);
		asset.ClearItems();
		/*asset.Screenshot.Asset = null;*/
	}

	private readonly HashSet<Asset> _assets = new();

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}