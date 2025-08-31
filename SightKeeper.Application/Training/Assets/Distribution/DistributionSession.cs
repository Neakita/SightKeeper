using SightKeeper.Application.Extensions;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Application.Training.Assets.Distribution;

internal sealed class DistributionSession<TAsset> where TAsset : ReadOnlyAsset
{
	public DistributionSession(IEnumerable<TAsset> assets)
	{
		_assets = assets
			.Where(asset => asset.Usage != AssetUsage.None)
			.Shuffle(0)
			.ToList();
		_totalAssetsCount = _assets.Count;
	}

	public IReadOnlyList<TAsset> PopAssetsFraction(AssetUsage usage, float fraction)
	{
		var count = (int)(_totalAssetsCount * fraction);
		return PopAssets(usage, count);
	}

	public List<TAsset> GetRemaining()
	{
		return _assets;
	}

	private readonly int _totalAssetsCount;
	private readonly List<TAsset> _assets;

	private List<TAsset> PopAssets(AssetUsage usage, int count)
	{
		// sort the assets so that the highest-priority ones are at the end for more efficient removals
		_assets.Sort(new ReverseComparer<TAsset>(new AssetUsageOrderComparer<TAsset>(usage)));
		var startIndex = _assets.Count - count;
		var assets = _assets.GetRange(startIndex, count);
		_assets.RemoveRange(startIndex, count);
		return assets;
	}
}