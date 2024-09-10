using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Poser3D;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Data.Binary.Replication.DataSets;

internal sealed class Poser3DDataSetReplicator : PoserDataSetReplicator<Poser3DTag, KeyPointTag3D>
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

	protected override PoserTag ReplicateTag(TagsLibrary library, PackableTag packed, ReplicationSession session)
	{
		var tag = (Poser3DTag)base.ReplicateTag(library, packed, session);
		var typedPackedTag = (PackablePoser3DTag)packed;
		foreach (var property in typedPackedTag.NumericProperties)
			tag.CreateNumericProperty(property.Name, property.MinimumValue, property.MaximumValue);
		foreach (var property in typedPackedTag.BooleanProperties)
			tag.CreateBooleanProperty(property.Name);
		return tag;
	}

	protected override void ReplicateAsset(AssetsLibrary library, PackableAsset packedAsset, Screenshot screenshot, ReplicationSession session)
	{
		var typedLibrary = (AssetsLibrary<Poser3DAsset>)library;
		var typedPackedAsset = (PackableItemsAsset<PackablePoser3DItem>)packedAsset;
		var asset = typedLibrary.MakeAsset((Screenshot<Poser3DAsset>)screenshot);
		foreach (var packedItem in typedPackedAsset.Items)
		{
			var itemTag = (Poser3DTag)session.Tags[(library.DataSet, packedItem.TagId)];
			asset.CreateItem(
				itemTag,
				packedItem.Bounding,
				packedItem.KeyPoints.Select(keyPoint => new KeyPoint3D(keyPoint.Position, keyPoint.IsVisible)).ToImmutableList(),
				packedItem.NumericProperties,
				packedItem.BooleanProperties);
		}
	}
}