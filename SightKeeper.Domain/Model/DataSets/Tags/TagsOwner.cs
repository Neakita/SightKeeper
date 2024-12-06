namespace SightKeeper.Domain.Model.DataSets.Tags;

public interface TagsOwner
{
	IReadOnlyList<Tag> Tags { get; }
}