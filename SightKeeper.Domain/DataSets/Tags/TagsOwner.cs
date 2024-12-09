namespace SightKeeper.Domain.DataSets.Tags;

internal interface TagsOwner
{
	IReadOnlyList<Tag> Tags { get; }
}