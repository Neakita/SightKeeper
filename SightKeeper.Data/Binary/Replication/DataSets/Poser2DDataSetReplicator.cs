using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Poser2D;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Data.Binary.Replication.DataSets;

internal sealed class Poser2DDataSetReplicator : PoserDataSetReplicator<Poser2DTag, KeyPointTag2D>
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

	protected override PoserTag ReplicateTag(TagsLibrary library, PackableTag packed, ReplicationSession session)
	{
		var tag = (Poser2DTag)base.ReplicateTag(library, packed, session);
		var typedPackedTag = (PackablePoser2DTag)packed;
		foreach (var property in typedPackedTag.NumericProperties)
			tag.CreateProperty(property.Name, property.MinimumValue, property.MaximumValue);
		return tag;
	}

	protected override void ReplicateAsset(AssetsLibrary library, PackableAsset packedAsset, Screenshot screenshot, ReplicationSession session)
	{
		var typedLibrary = (AssetsLibrary<Poser2DAsset>)library;
		var typedPackedAsset = (PackableItemsAsset<PackablePoser2DItem>)packedAsset;
		var asset = typedLibrary.MakeAsset((Screenshot<Poser2DAsset>)screenshot);
		foreach (var packedItem in typedPackedAsset.Items)
		{
			var itemTag = (Poser2DTag)session.Tags[(library.DataSet, packedItem.TagId)];
			asset.CreateItem(
				itemTag,
				packedItem.Bounding,
				packedItem.KeyPoints.Select(keyPoint => keyPoint.Position).ToImmutableList(),
				packedItem.NumericProperties);
		}
	}
}