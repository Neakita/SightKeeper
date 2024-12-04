namespace SightKeeper.Domain.Model.DataSets.Tags;

public interface TagsOwner
{
	IReadOnlyCollection<Tag> Tags { get; }
}