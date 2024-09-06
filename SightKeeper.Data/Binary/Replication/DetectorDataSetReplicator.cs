using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Data.Binary.Replication;

internal sealed class DetectorDataSetReplicator : DataSetReplicator
{
	public DetectorDataSetReplicator(FileSystemScreenshotsDataAccess screenshotsDataAccess) : base(screenshotsDataAccess)
	{
	}

	protected override DetectorDataSet CreateDataSet(string name, string description, Game? game, Composition? composition)
	{
		return new DetectorDataSet
		{
			Name = name,
			Description = description,
			Game = game,
			Composition = composition
		};
	}

	protected override void ReplicateAsset(AssetsLibrary library, PackableAsset packedAsset, Screenshot screenshot, TagGetter getTag)
	{
		var typedLibrary = (AssetsLibrary<DetectorAsset>)library;
		var typedPackedAsset = (PackableItemsAsset<PackableDetectorItem>)packedAsset;
		var asset = typedLibrary.MakeAsset((Screenshot<DetectorAsset>)screenshot);
		foreach (var packedItem in typedPackedAsset.Items)
		{
			var itemTag = (DetectorTag)getTag(packedItem.TagId);
			asset.CreateItem(itemTag, packedItem.Bounding);
		}
	}
}