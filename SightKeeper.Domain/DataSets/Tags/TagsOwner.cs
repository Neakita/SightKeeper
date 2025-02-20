namespace SightKeeper.Domain.DataSets.Tags;

public interface TagsOwner
{
	IReadOnlyList<Tag> Tags { get; }
}