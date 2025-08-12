using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Application.Training.Assets.Distribution;

internal sealed class AssetUsageOrderComparer<TAsset> : IComparer<TAsset> where TAsset : Asset
{
	public AssetUsageOrderComparer(AssetUsage targetUsage)
	{
		_targetUsage = targetUsage;
	}

	public int Compare(TAsset? x, TAsset? y)
	{
		if (ReferenceEquals(x, y)) return 0;
		if (y is null) return 1;
		if (x is null) return -1;
		var xUsageOrder = GetOrder(x.Usage);
		var yUsageOrder = GetOrder(y.Usage);
		return xUsageOrder.CompareTo(yUsageOrder);
	}

	private readonly AssetUsage _targetUsage;

	private int GetOrder(AssetUsage assetUsage)
	{
		if (assetUsage == _targetUsage)
			return 0;
		if (assetUsage.HasFlag(_targetUsage))
			return 1;
		return 2;
	}
}