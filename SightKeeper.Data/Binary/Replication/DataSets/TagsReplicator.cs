using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.Binary.Replication.DataSets;

internal static class TagsReplicator
{
	public static void ReplicateTags(TagsLibrary<Tag> library, IReadOnlyCollection<PackableTag> packableTags)
	{
		foreach (var packableTag in packableTags)
		{
			var tag = library.CreateTag(packableTag.Name);
			tag.Color = packableTag.Color;
		}
	}
}