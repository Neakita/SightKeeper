using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Replication.DataSets;

internal sealed class ClassifierDataSetReplicator : DataSetReplicator<ClassifierDataSet>
{
	public ClassifierDataSetReplicator(FileSystemScreenshotsDataAccess screenshotsDataAccess, ReplicationSession session) : base(screenshotsDataAccess, session)
	{
	}

	protected override void ReplicateAsset(AssetsLibrary library, PackableAsset packedAsset, Screenshot screenshot)
	{
		var typedPackedAsset = (PackableClassifierAsset)packedAsset;
		var typedLibrary = (AssetsLibrary<ClassifierAsset>)library;
		var asset = typedLibrary.MakeAsset((Screenshot<ClassifierAsset>)screenshot);
		asset.Tag = (ClassifierTag)Session.Tags[(library.DataSet, typedPackedAsset.TagId)];
	}

	protected override PlainWeights<ClassifierTag> ReplicateWeights(WeightsLibrary library, PackableWeights weights)
	{
		var typedLibrary = (WeightsLibrary<ClassifierTag>)library;
		var typedWeights = (PackablePlainWeights)weights;
		var tags = typedWeights.TagIds.Select(id => Session.Tags[(library.DataSet, id)]).Cast<ClassifierTag>();
		return typedLibrary.CreateWeights(weights.CreationDate, weights.ModelSize, weights.Metrics, weights.Resolution, tags);
	}
}