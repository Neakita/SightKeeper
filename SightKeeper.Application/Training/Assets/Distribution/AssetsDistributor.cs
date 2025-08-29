using SightKeeper.Application.Training.Data;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Application.Training.Assets.Distribution;

public static class AssetsDistributor
{
	public static AssetsDistribution<TAsset> DistributeAssets<TAsset>(IEnumerable<TAsset> assets, AssetsDistributionRequest request) where TAsset : AssetData
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