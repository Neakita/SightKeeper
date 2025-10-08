using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Application.Training.Assets.Distribution;

internal static class AssetsDistributor
{
	public static AssetsDistribution<TAsset> DistributeAssets<TAsset>(IEnumerable<TAsset> assets, AssetsDistributionRequest request) where TAsset : ReadOnlyAsset
	{
		DistributionSession<TAsset> session = new(assets);
		request = request.Normalized;
		return new AssetsDistribution<TAsset>
		{
			TrainAssets = session.PopAssetsFraction(AssetUsage.Train, request.TrainFraction),
			ValidationAssets = session.PopAssetsFraction(AssetUsage.Validation, request.ValidationFraction),
			TestAssets = session.GetRemaining()
		};
	}
}