using System.Collections;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets;

public sealed class AssetsLibrary : IReadOnlyCollection<Asset>
{
	public int Count => _assets.Count;
	public DataSet DataSet { get; }

	public AssetsLibrary(DataSet dataSet)
	{
		DataSet = dataSet;
		_assets = new HashSet<Asset>();
	}
	public IEnumerator<Asset> GetEnumerator()
	{
		return _assets.GetEnumerator();
	}

	public Asset MakeAsset(Screenshot screenshot)
	{
		Guard.IsNull(screenshot.Asset);
		screenshot.Asset = new Asset(screenshot);
		AddAsset(screenshot.Asset);
		return screenshot.Asset;
	}

	public void DeleteAsset(Asset asset)
	{
		var isRemoved = _assets.Remove(asset);
		Guard.IsTrue(isRemoved);
		asset.ClearItems();
		asset.Screenshot.Asset = null;
	}

	internal void AddAsset(Asset asset)
	{
		Guard.IsReferenceEqualTo(asset.Screenshot.Library.DataSet, DataSet);
		bool isAdded = _assets.Add(asset);
		Guard.IsTrue(isAdded);
	}

	private readonly HashSet<Asset> _assets;

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}