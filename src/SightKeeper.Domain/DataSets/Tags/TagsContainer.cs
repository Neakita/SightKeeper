namespace SightKeeper.Domain.DataSets.Tags;

public interface TagsContainer<out TTag>
{
	IReadOnlyList<TTag> Tags { get; }
}