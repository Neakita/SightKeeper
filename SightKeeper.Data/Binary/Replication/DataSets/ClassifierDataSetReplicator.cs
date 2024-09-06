using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Replication.DataSets;

internal sealed class ClassifierDataSetReplicator : DataSetReplicator
{
	public ClassifierDataSetReplicator(FileSystemScreenshotsDataAccess screenshotsDataAccess) : base(screenshotsDataAccess)
	{
	}

	protected override ClassifierDataSet CreateDataSet(string name, string description, Game? game, Composition? composition)
	{
		return new ClassifierDataSet
		{
			Name = name,
			Description = description,
			Game = game,
			Composition = composition
		};
	}

	protected override void ReplicateAsset(AssetsLibrary library, PackableAsset packedAsset, Screenshot screenshot, TagGetter getTag)
	{
		var typedPackedAsset = (PackableClassifierAsset)packedAsset;
		var typedLibrary = (AssetsLibrary<ClassifierAsset>)library;
		var asset = typedLibrary.MakeAsset((Screenshot<ClassifierAsset>)screenshot);
		asset.Tag = (ClassifierTag)getTag(typedPackedAsset.TagId);
	}

	protected override void ReplicateWeights(WeightsLibrary library, PackableWeights weights, TagGetter getTag)
	{
		var typedLibrary = (WeightsLibrary<ClassifierTag>)library;
		var typedWeights = (PackablePlainWeights)weights;
		var tags = typedWeights.TagIds.Select(id => getTag(id)).Cast<ClassifierTag>();
		typedLibrary.CreateWeights(weights.CreationDate, weights.ModelSize, weights.Metrics, weights.Resolution, tags);
	}
}