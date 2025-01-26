namespace SightKeeper.Domain.DataSets.Tags;

public interface TagsContainer<out TTag>
{
	IReadOnlyCollection<TTag> Tags { get; }
}