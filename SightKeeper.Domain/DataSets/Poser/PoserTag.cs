using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser;

public sealed class PoserTag : Tag, TagsOwner<Tag>
{
	public IReadOnlyList<Tag> KeyPointTags => _keyPointTags.AsReadOnly();
	IReadOnlyList<Tag> TagsContainer<Tag>.Tags => KeyPointTags;

	public Tag CreateKeyPointTag(string name)
	{
		Tag tag = new(this, name);
		_keyPointTags.Add(tag);
		return tag;
	}

	public void DeleteKeyPointTagAt(int index)
	{
		var tag = _keyPointTags[index];
		var isTagInUse = _usageProvider.IsInUse(tag);
		if (isTagInUse)
			TagIsInUseException.ThrowForDeletion(tag);
		_keyPointTags.RemoveAt(index);
	}

	public void DeleteKeyPointTag(Tag tag)
	{
		var isTagInUse = _usageProvider.IsInUse(tag);
		if (isTagInUse)
			TagIsInUseException.ThrowForDeletion(tag);
		var isRemoved = _keyPointTags.Remove(tag);
		if (!isRemoved)
			throw new ArgumentException("Specified tag was not found and therefore not deleted", nameof(tag));
	}

	Tag TagsOwner<Tag>.CreateTag(string name)
	{
		return CreateKeyPointTag(name);
	}

	void TagsOwner<Tag>.DeleteTagAt(int index)
	{
		DeleteKeyPointTagAt(index);
	}

	internal PoserTag(TagsContainer<Tag> owner, string name, TagsUsageProvider usageProvider) : base(owner, name)
	{
		_usageProvider = usageProvider;
	}

	private readonly TagsUsageProvider _usageProvider;
	private readonly List<Tag> _keyPointTags = new();
}