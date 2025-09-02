namespace SightKeeper.Domain.DataSets.Tags;

public sealed class DomainTagsLibrary<TTag> : TagsOwner<TTag> where TTag : Tag
{
	public IReadOnlyList<TTag> Tags => _inner.Tags;

	public TTag CreateTag(string name)
	{
		if (Tags.Any(tag => tag.Name == name))
			throw new ArgumentException($"Tag name \"{name}\" is already used", nameof(name));
		return _inner.CreateTag(name);
	}

	public void DeleteTagAt(int index)
	{
		var tag = _inner.Tags[index];
		TagIsInUseException.ThrowForDeletionIfInUse(tag);
		_inner.DeleteTagAt(index);
	}

	public DomainTagsLibrary(TagsOwner<TTag> inner)
	{
		_inner = inner;
	}

	private readonly TagsOwner<TTag> _inner;
}