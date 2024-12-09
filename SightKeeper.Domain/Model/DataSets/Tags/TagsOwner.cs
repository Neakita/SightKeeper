namespace SightKeeper.Domain.Model.DataSets.Tags;

internal interface TagsOwner
{
	IReadOnlyList<Tag> Tags { get; }
}