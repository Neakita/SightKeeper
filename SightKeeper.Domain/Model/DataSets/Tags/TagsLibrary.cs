using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Tags;

public abstract class TagsLibrary : TagsHolder
{
	public abstract int Count { get; }
	public abstract DataSet DataSet { get; }
	public abstract IReadOnlyCollection<Tag> Tags { get; }

	public abstract Tag CreateTag(string name);
}

public sealed class TagsLibrary<TTag> : TagsLibrary where TTag : Tag, TagsFactory<TTag>
{
	public override int Count => _tags.Count;
	public override DataSet DataSet { get; }
	public override IReadOnlyCollection<TTag> Tags => _tags;

	public TagsLibrary(DataSet dataSet)
	{
		DataSet = dataSet;
	}

	public override TTag CreateTag(string name)
	{
		var tag = TTag.Create(name, this);
		AddTag(tag);
		return tag;
	}

	public void DeleteTag(TTag tag)
	{
		Guard.IsFalse(tag.IsInUse);
		Guard.IsTrue(_tags.Remove(tag));
	}

	private void AddTag(TTag tag)
	{
		foreach (var existingTag in _tags)
			Guard.IsNotEqualTo(existingTag.Name, tag.Name);
		_tags.Add(tag);
	}

	private readonly List<TTag> _tags = new();
}