using System.Collections.Immutable;
using FlakeId;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Poser2D;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Data.Binary.Replication;

internal sealed class Poser2DDataSetReplicator : DataSetReplicator
{
	public Poser2DDataSetReplicator(FileSystemScreenshotsDataAccess screenshotsDataAccess) : base(screenshotsDataAccess)
	{
	}

	protected override Poser2DDataSet CreateDataSet(string name, string description, Game? game, Composition? composition)
	{
		return new Poser2DDataSet
		{
			Name = name,
			Description = description,
			Game = game,
			Composition = composition
		};
	}

	protected override void ReplicateAsset(AssetsLibrary library, PackableAsset packedAsset, Func<Id, Screenshot> getScreenshot, TagGetter getTag)
	{
		var typedLibrary = (AssetsLibrary<Poser2DAsset>)library;
		var typedPackedAsset = (PackableItemsAsset<PackablePoser2DItem>)packedAsset;
		var screenshot = (Screenshot<Poser2DAsset>)getScreenshot(packedAsset.ScreenshotId);
		var asset = typedLibrary.MakeAsset(screenshot);
		foreach (var packedItem in typedPackedAsset.Items)
		{
			var itemTag = (Poser2DTag)getTag(packedItem.TagId);
			asset.CreateItem(
				itemTag,
				packedItem.Bounding,
				packedItem.KeyPoints.Select(keyPoint => keyPoint.Position).ToImmutableList(),
				packedItem.NumericProperties);
		}
	}
}