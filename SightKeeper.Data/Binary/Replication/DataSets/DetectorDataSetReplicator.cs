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
	public DetectorDataSetReplicator(FileSystemScreenshotsDataAccess screenshotsDataAccess, ReplicationSession session) : base(screenshotsDataAccess, session)
	{
	}

	protected override void ReplicateAsset(AssetsLibrary library, PackableAsset packedAsset, Screenshot screenshot)
	{
		var typedLibrary = (AssetsLibrary<DetectorAsset>)library;
		var typedPackedAsset = (PackableItemsAsset<PackableDetectorItem>)packedAsset;
		var asset = typedLibrary.MakeAsset((Screenshot<DetectorAsset>)screenshot);
		foreach (var packedItem in typedPackedAsset.Items)
		{
			var itemTag = (DetectorTag)Session.Tags[(library.DataSet, packedItem.TagId)];
			asset.CreateItem(itemTag, packedItem.Bounding);
		}
	}

	protected override PlainWeights<DetectorTag> ReplicateWeights(WeightsLibrary library, PackableWeights weights)
	{
		var typedLibrary = (WeightsLibrary<DetectorTag>)library;
		var tags = weights.TagIds.Select(id => Session.Tags[(library.DataSet, id)]).Cast<DetectorTag>();
		return typedLibrary.CreateWeights(weights.CreationDate, weights.ModelSize, weights.Metrics, weights.Resolution, tags);
	}
}