using System.Collections;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorAssetsLibrary : IReadOnlyCollection<DetectorAsset>
{
	public int Count => _assets.Count;

	public IEnumerator<DetectorAsset> GetEnumerator()
	{
		return _assets.GetEnumerator();
	}

	public DetectorAsset MakeAsset(Screenshot screenshot)
	{
		// TODO
		/*Guard.IsNull(screenshot.Asset);
		screenshot.Asset = new Asset(screenshot);*/
		DetectorAsset asset = new(screenshot);
		bool isAdded = _assets.Add(asset);
		Guard.IsTrue(isAdded);
		return asset;
	}

	public void DeleteAsset(DetectorAsset asset)
	{
		// TODO
		var isRemoved = _assets.Remove(asset);
		Guard.IsTrue(isRemoved);
		asset.ClearItems();
		/*asset.Screenshot.Asset = null;*/
	}

	private readonly HashSet<DetectorAsset> _assets = new();

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}