namespace SightKeeper.Domain.DataSets.Tags;

public abstract class TagsLibrary : TagsOwner
{
	public abstract IReadOnlyList<Tag> Tags { get; }

	public abstract Tag CreateTag(string name);
}