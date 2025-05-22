using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.Tests.DataSets.Fakes;

internal sealed class FakeTagsChanges : TagsChanges
{
	public IEnumerable<Tag> RemovedTags { get; init; } = Enumerable.Empty<Tag>();
	public IEnumerable<EditedTagData> EditedTags { get; init; } = Enumerable.Empty<EditedTagData>();
	public IEnumerable<NewTagData> NewTags { get; init; } = Enumerable.Empty<NewTagData>();
}