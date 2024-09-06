using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Data.Binary.Replication;

internal abstract class PoserDataSetReplicator : DataSetReplicator
{
	protected PoserDataSetReplicator(FileSystemScreenshotsDataAccess screenshotsDataAccess) : base(screenshotsDataAccess)
	{
	}

	protected override PoserTag ReplicateTag(TagsLibrary library, PackableTag packed, ImmutableDictionary<(byte, byte?), Tag>.Builder lookupBuilder)
	{
		var typedPackedTag = (PackablePoserTag)packed;
		var tag = (PoserTag)base.ReplicateTag(library, packed, lookupBuilder);
		foreach (var packedKeyPointTag in typedPackedTag.KeyPointTags)
		{
			var keyPointTag = tag.CreateKeyPoint(typedPackedTag.Name);
			keyPointTag.Color = packedKeyPointTag.Color;
			lookupBuilder.Add((typedPackedTag.Id, packedKeyPointTag.Id), keyPointTag);
		}
		return tag;
	}
}