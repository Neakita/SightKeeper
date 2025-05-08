namespace SightKeeper.Domain.DataSets.Tags;

public sealed class TagsLibrary<TTag> : TagsLibrary, TagsOwner<TTag> where TTag : Tag
{
	public override IReadOnlyList<TTag> Tags => _tags.AsReadOnly();
	IReadOnlyList<TTag> TagsContainer<TTag>.Tags => Tags;

	public override TTag CreateTag(string name)
	{
		var tag = _tagsFactory.CreateTag(this, name);
		_tags.Add(tag);
		return tag;
	}

	public override void DeleteTagAt(int index)
	{
		var tag = _tags[index];
		TagIsInUseException.ThrowForDeletionIfInUse(tag);
		_tags.RemoveAt(index);
	}

	public void DeleteTag(TTag tag)
	{
		TagIsInUseException.ThrowForDeletionIfInUse(tag);
		var isRemoved = _tags.Remove(tag);
		if (!isRemoved)
			throw new ArgumentException("Specified tag was not found and therefore not deleted", nameof(tag));
	}

	internal TagsLibrary(TagsFactory<TTag> tagsFactory)
	{
		_tagsFactory = tagsFactory;
	}

	private readonly TagsFactory<TTag> _tagsFactory;
	private readonly List<TTag> _tags = new();
}