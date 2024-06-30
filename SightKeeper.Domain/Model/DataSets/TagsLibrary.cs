using System.Collections;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets;

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

public abstract class TagsLibrary<TTag> : TagsLibrary, IReadOnlyCollection<TTag> where TTag : Tag
{
	public override int Count => _tags.Count;

	public virtual void DeleteTag(TTag tag)
	{
		bool isRemoved = _tags.Remove(tag);
		Guard.IsTrue(isRemoved);
	}

	public override IEnumerator<TTag> GetEnumerator()
	{
		return _tags.GetEnumerator();
	}

	protected void AddTag(TTag tag)
	{
		foreach (var existingTag in _tags)
			Guard.IsNotEqualTo(existingTag.Name, tag.Name);
		_tags.Add(tag);
	}

	private readonly List<TTag> _tags = new();
}