using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.DataSets.Tags;

public interface TagsChanges
{
	IEnumerable<Tag> RemovedTags { get; }
	IEnumerable<EditedTagData> EditedTags { get; }
	IEnumerable<NewTagData> NewTags { get; }
}