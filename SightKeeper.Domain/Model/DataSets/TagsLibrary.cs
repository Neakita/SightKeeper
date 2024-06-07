using System.Collections;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets;

public abstract class TagsLibrary<TTag> : IReadOnlyCollection<TTag> where TTag : Tag
{
	public int Count => _tags.Count;

	public virtual void DeleteTag(TTag tag)
	{
		bool isRemoved = _tags.Remove(tag);
		Guard.IsTrue(isRemoved);
	}

	public IEnumerator<TTag> GetEnumerator()
	{
		return _tags.GetEnumerator();
	}

	protected void AddTag(TTag tag)
	{
		bool isNameAlreadyUsed = _tags.Any(existingTag => existingTag.Name == tag.Name);
		Guard.IsFalse(isNameAlreadyUsed);
		_tags.Add(tag);
	}

	private readonly List<TTag> _tags = new();

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}