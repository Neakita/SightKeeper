using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Poser2D;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Data.Binary.Replication.DataSets;

internal sealed class Poser2DDataSetReplicator : PoserDataSetReplicator<Poser2DTag, KeyPointTag2D, Poser2DDataSet>
{
	public Poser2DDataSetReplicator(FileSystemScreenshotsDataAccess screenshotsDataAccess, ReplicationSession session) : base(screenshotsDataAccess, session)
	{
	}

	protected override PoserTag ReplicateTag(TagsLibrary library, PackableTag packed)
	{
		var tag = (Poser2DTag)base.ReplicateTag(library, packed);
		var typedPackedTag = (PackablePoser2DTag)packed;
		foreach (var property in typedPackedTag.NumericProperties)
			tag.CreateProperty(property.Name, property.MinimumValue, property.MaximumValue);
		return tag;
	}

	protected override void ReplicateAsset(AssetsLibrary library, PackableAsset packedAsset, Screenshot screenshot)
	{
		var typedLibrary = (AssetsLibrary<Poser2DAsset>)library;
		var typedPackedAsset = (PackableItemsAsset<PackablePoser2DItem>)packedAsset;
		var asset = typedLibrary.MakeAsset((Screenshot<Poser2DAsset>)screenshot);
		foreach (var packedItem in typedPackedAsset.Items)
		{
			var itemTag = (Poser2DTag)Session.Tags[(library.DataSet, packedItem.TagId)];
			var item = asset.CreateItem(
				itemTag,
				packedItem.Bounding,
				packedItem.NumericProperties);
			foreach (var packedKeyPoint in packedItem.KeyPoints)
			{
				var keyPointTag = itemTag.KeyPoints[packedKeyPoint.Index];
				item.CreateKeyPoint(keyPointTag, packedKeyPoint.Position);
			}
		}
	}
}