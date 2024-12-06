using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Tags;

public abstract class TagsLibrary : TagsOwner
{
	public abstract IReadOnlyList<Tag> Tags { get; }

	public abstract Tag CreateTag(string name);
}

public sealed class TagsLibrary<TTag> : TagsLibrary where TTag : Tag
{
	public override IReadOnlyList<TTag> Tags => _tags.AsReadOnly();

	public TagsLibrary(TagsFactory<TTag> tagsFactory, TagsUsageProvider tagsUsageProvider)
	{
		_tagsFactory = tagsFactory;
		_tagsUsageProvider = tagsUsageProvider;
	}

	public override TTag CreateTag(string name)
	{
		var tag = _tagsFactory.CreateTag(this, name);
		_tags.Add(tag);
		return tag;
	}

	public void DeleteTag(TTag tag)
	{
		bool isTagInUse = _tagsUsageProvider.IsInUse(tag);
		Guard.IsFalse(isTagInUse);
		var isRemoved = _tags.Remove(tag);
		Guard.IsTrue(isRemoved);
	}

	private readonly TagsFactory<TTag> _tagsFactory;
	private readonly TagsUsageProvider _tagsUsageProvider;
	private readonly List<TTag> _tags = new();
}