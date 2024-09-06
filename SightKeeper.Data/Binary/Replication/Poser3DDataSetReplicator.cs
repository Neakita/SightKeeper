using System.Collections.Immutable;
using FlakeId;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Poser3D;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Data.Binary.Replication;

internal sealed class Poser3DDataSetReplicator : DataSetReplicator
{
	public Poser3DDataSetReplicator(FileSystemScreenshotsDataAccess screenshotsDataAccess) : base(screenshotsDataAccess)
	{
	}


	protected override Poser3DDataSet CreateDataSet(string name, string description, Game? game, Composition? composition)
	{
		return new Poser3DDataSet
		{
			Name = name,
			Description = description,
			Game = game,
			Composition = composition
		};
	}

	protected override void ReplicateAsset(AssetsLibrary library, PackableAsset packedAsset, Func<Id, Screenshot> getScreenshot, TagGetter getTag)
	{
		var typedLibrary = (AssetsLibrary<Poser3DAsset>)library;
		var typedPackedAsset = (PackableItemsAsset<PackablePoser3DItem>)packedAsset;
		var screenshot = (Screenshot<Poser3DAsset>)getScreenshot(packedAsset.ScreenshotId);
		var asset = typedLibrary.MakeAsset(screenshot);
		foreach (var packedItem in typedPackedAsset.Items)
		{
			var itemTag = (Poser3DTag)getTag(packedItem.TagId);
			asset.CreateItem(
				itemTag,
				packedItem.Bounding,
				packedItem.KeyPoints.Select(keyPoint => new KeyPoint3D(keyPoint.Position, keyPoint.IsVisible)).ToImmutableList(),
				packedItem.NumericProperties,
				packedItem.BooleanProperties);
		}
	}
}