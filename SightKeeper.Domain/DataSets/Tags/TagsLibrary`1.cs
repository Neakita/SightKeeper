namespace SightKeeper.Domain.DataSets.Tags;

public sealed class TagsLibrary<TTag> : TagsLibrary, TagsContainer<TTag> where TTag : Tag
{
	public override IReadOnlyList<TTag> Tags => _tags.AsReadOnly();
	IReadOnlyCollection<TTag> TagsContainer<TTag>.Tags => Tags;

	public override TTag CreateTag(string name)
	{
		var tag = _tagsFactory.CreateTag(this, name);
		_tags.Add(tag);
		return tag;
	}

	public void DeleteTag(TTag tag)
	{
		bool isTagInUse = _tagsUsageProvider.IsInUse(tag);
		if (isTagInUse)
			TagIsInUseException.ThrowForDeletion(tag);
		var isRemoved = _tags.Remove(tag);
		if (!isRemoved)
			throw new ArgumentException("Specified tag was not found and therefore not deleted", nameof(tag));
	}

	internal TagsLibrary(TagsFactory<TTag> tagsFactory, TagsUsageProvider tagsUsageProvider)
	{
		_tagsFactory = tagsFactory;
		_tagsUsageProvider = tagsUsageProvider;
	}

	private readonly TagsFactory<TTag> _tagsFactory;
	private readonly TagsUsageProvider _tagsUsageProvider;
	private readonly List<TTag> _tags = new();
}