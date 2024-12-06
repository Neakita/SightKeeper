using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Data.Binary.Replication.DataSets.Poser;

internal static class PoserTagsReplicator
{
	public static void ReplicateTags(TagsLibrary<PoserTag> library, IReadOnlyCollection<PackablePoserTag> packableTags)
	{
		foreach (var packableTag in packableTags)
		{
			var tag = library.CreateTag(packableTag.Name);
			tag.Color = packableTag.Color;
			ReplicateKeyPointTags(tag, packableTag.KeyPointTags);
		}
	}

	private static void ReplicateKeyPointTags(PoserTag poserTag, IReadOnlyCollection<PackableTag> packableTags)
	{
		foreach (var packableTag in packableTags)
		{
			var keyPointTag = poserTag.CreateKeyPointTag(packableTag.Name);
			keyPointTag.Color = packableTag.Color;
		}
	}
}