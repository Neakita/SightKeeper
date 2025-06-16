namespace SightKeeper.Domain.DataSets.Tags;

public interface Tag
{
	string Name { get; set; }
	uint Color { get; set; }
	TagsContainer<Tag> Owner { get; }
	IReadOnlyCollection<TagUser> Users { get; }
}