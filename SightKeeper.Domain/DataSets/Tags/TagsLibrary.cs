namespace SightKeeper.Domain.DataSets.Tags;

public abstract class TagsLibrary : TagsOwner<Tag>
{
	public required DataSet DataSet { get; init; }
	public abstract IReadOnlyList<Tag> Tags { get; }

	public abstract Tag CreateTag(string name);
	public abstract void DeleteTagAt(int index);
}