using System.Collections;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Tags;

public abstract class TagsLibrary : IReadOnlyCollection<Tag>
{
	public abstract int Count { get; }
	public abstract DataSet DataSet { get; }

	public abstract IEnumerator<Tag> GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}

public sealed class TagsLibrary<TTag> : TagsLibrary, IReadOnlyCollection<TTag> where TTag : Tag, TagsFactory<TTag>
{
	public override int Count => _tags.Count;
	public override DataSet DataSet { get; }

	public TagsLibrary(DataSet dataSet)
	{
		DataSet = dataSet;
	}

	public TTag CreateTag(string name)
	{
		var tag = TTag.Create(name, this);
		AddTag(tag);
		return tag;
	}

	public void DeleteTag(TTag tag)
	{
		Guard.IsTrue(tag.CanDelete);
		Guard.IsTrue(_tags.Remove(tag));
	}

	public override IEnumerator<TTag> GetEnumerator()
	{
		return _tags.GetEnumerator();
	}

	private void AddTag(TTag tag)
	{
		foreach (var existingTag in _tags)
			Guard.IsNotEqualTo(existingTag.Name, tag.Name);
		_tags.Add(tag);
	}

	private readonly List<TTag> _tags = new();
}