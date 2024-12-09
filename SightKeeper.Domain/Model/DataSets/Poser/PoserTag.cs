using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class PoserTag : Tag, TagsOwner
{
	public IReadOnlyList<Tag> KeyPointTags => _keyPointTags.AsReadOnly();
	IReadOnlyList<Tag> TagsOwner.Tags => KeyPointTags;

	public Tag CreateKeyPointTag(string name)
	{
		Tag tag = new(this, name);
		_keyPointTags.Add(tag);
		return tag;
	}

	public void DeleteKeyPointTag(Tag tag)
	{
		var isTagInUse = _usageProvider.IsInUse(tag);
		if (isTagInUse)
			TagIsInUseException.ThrowForDeletion(tag);
		var isRemoved = _keyPointTags.Remove(tag);
		Guard.IsTrue(isRemoved);
	}

	internal PoserTag(TagsOwner owner, string name, TagsUsageProvider usageProvider) : base(owner, name)
	{
		_usageProvider = usageProvider;
	}

	private readonly TagsUsageProvider _usageProvider;
	private readonly List<Tag> _keyPointTags = new();
}