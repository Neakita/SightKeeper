namespace SightKeeper.Data.DataSets.Tags;

internal sealed class InMemoryTagsLibrary<TTag>(TagFactory<TTag> tagFactory) : StorableTagsOwner<TTag>
{
	public IReadOnlyList<TTag> Tags => _tags;

	public TTag CreateTag(string name)
	{
		var tag = tagFactory.CreateTag(name);
		_tags.Add(tag);
		return tag;
	}

	public void DeleteTagAt(int index)
	{
		_tags.RemoveAt(index);
	}

	public void EnsureCapacity(int capacity)
	{
		_tags.EnsureCapacity(capacity);
	}

	private readonly List<TTag> _tags = new();
}