using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Application.Training.Assets.Distribution;

internal sealed class DistributionSession
{
	public DistributionSession(IEnumerable<Asset> assets)
	{
		_assets = assets.ToList();
	}

	public IReadOnlyCollection<Asset> PopAssets(AssetUsage usage, int count)
	{
		// sort the assets so that the highest-priority ones are at the end for more efficient removals
		_assets.Sort(new ReverseComparer<Asset>(new AssetUsageOrderComparer(usage)));
		var startIndex = _assets.Count - count;
		var assets = _assets.GetRange(startIndex, count);
		_assets.RemoveRange(startIndex, count);
		return assets;
	}

	private readonly List<Asset> _assets;
}