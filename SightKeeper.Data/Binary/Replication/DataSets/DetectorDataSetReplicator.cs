using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Replication.DataSets;

internal sealed class DetectorDataSetReplicator : DataSetReplicator<DetectorDataSet>
{
	public DetectorDataSetReplicator(FileSystemScreenshotsDataAccess screenshotsDataAccess) : base(screenshotsDataAccess)
	{
	}

	protected override void ReplicateAsset(AssetsLibrary library, PackableAsset packedAsset, Screenshot screenshot, ReplicationSession session)
	{
		var typedLibrary = (AssetsLibrary<DetectorAsset>)library;
		var typedPackedAsset = (PackableItemsAsset<PackableDetectorItem>)packedAsset;
		var asset = typedLibrary.MakeAsset((Screenshot<DetectorAsset>)screenshot);
		foreach (var packedItem in typedPackedAsset.Items)
		{
			var itemTag = (DetectorTag)session.Tags[(library.DataSet, packedItem.TagId)];
			asset.CreateItem(itemTag, packedItem.Bounding);
		}
	}

	protected override PlainWeights<DetectorTag> ReplicateWeights(WeightsLibrary library, PackableWeights weights, ReplicationSession session)
	{
		var typedLibrary = (WeightsLibrary<DetectorTag>)library;
		var typedWeights = (PackablePlainWeights)weights;
		var tags = typedWeights.TagIds.Select(id => session.Tags[(library.DataSet, id)]).Cast<DetectorTag>();
		return typedLibrary.CreateWeights(weights.CreationDate, weights.ModelSize, weights.Metrics, weights.Resolution, tags);
	}
}