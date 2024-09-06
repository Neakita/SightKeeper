using FlakeId;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Data.Binary.Replication;

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

	protected override void ReplicateAsset(AssetsLibrary library, PackableAsset packedAsset, Func<Id, Screenshot> getScreenshot, TagGetter getTag)
	{
		var typedPackedAsset = (PackableClassifierAsset)packedAsset;
		var typedLibrary = (AssetsLibrary<ClassifierAsset>)library;
		var screenshot = (Screenshot<ClassifierAsset>)getScreenshot(packedAsset.ScreenshotId);
		var asset = typedLibrary.MakeAsset(screenshot);
		asset.Tag = (ClassifierTag)getTag(typedPackedAsset.TagId);
	}
}