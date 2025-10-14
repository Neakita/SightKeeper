using System.Collections.ObjectModel;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Tags;

internal sealed class InMemoryTag : Tag
{
	public string Name { get; set; } = string.Empty;
	public uint Color { get; set; }
	public required TagsContainer<Tag> Owner { get; init; }
	public IReadOnlyCollection<TagUser> Users => ReadOnlyCollection<TagUser>.Empty;
}