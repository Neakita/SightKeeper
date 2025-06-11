namespace SightKeeper.Domain.DataSets.Tags;

public sealed class DomainTagsLibrary<TTag> : TagsOwner<TTag>, Decorator<TagsOwner<TTag>> where TTag : Tag
{
	public TagsOwner<TTag> Inner { get; }
	public IReadOnlyList<TTag> Tags => Inner.Tags;

	public TTag CreateTag(string name)
	{
		if (Tags.Any(tag => tag.Name == name))
			throw new ArgumentException($"Tag name \"{name}\" is already used", nameof(name));
		return Inner.CreateTag(name);
	}

	public void DeleteTagAt(int index)
	{
		var tag = Inner.Tags[index];
		TagIsInUseException.ThrowForDeletionIfInUse(tag);
		Inner.DeleteTagAt(index);
	}

	internal DomainTagsLibrary(TagsOwner<TTag> inner)
	{
		Inner = inner;
	}
}