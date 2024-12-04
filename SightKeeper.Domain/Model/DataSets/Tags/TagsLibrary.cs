using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Tags;

public abstract class TagsLibrary : TagsOwner
{
	public abstract IReadOnlyCollection<Tag> Tags { get; }

	public abstract Tag CreateTag(string name);
}

public sealed class TagsLibrary<TTag> : TagsLibrary where TTag : Tag
{
	public override IReadOnlyCollection<TTag> Tags => _tags;

	public TagsLibrary(TagsFactory<TTag> tagsFactory, TagsUsageProvider tagsUsageProvider)
	{
		_tagsFactory = tagsFactory;
		_tagsUsageProvider = tagsUsageProvider;
	}

	public override TTag CreateTag(string name)
	{
		var tag = _tagsFactory.CreateTag(this, name);
		AddTag(tag);
		return tag;
	}

	public void DeleteTag(TTag tag)
	{
		bool isTagInUse = _tagsUsageProvider.IsInUse(tag);
		Guard.IsFalse(isTagInUse);
		var isRemoved = _tags.Remove(tag);
		Guard.IsTrue(isRemoved);
	}

	private void AddTag(TTag tag)
	{
		bool isAdded = _tags.Add(tag);
		Guard.IsTrue(isAdded);
	}

	private readonly TagsFactory<TTag> _tagsFactory;
	private readonly TagsUsageProvider _tagsUsageProvider;
	private readonly HashSet<TTag> _tags = new();
}