﻿using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser;

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
		if (!isRemoved)
			throw new ArgumentException("Specified tag was not found and therefore not deleted", nameof(tag));
	}

	internal PoserTag(TagsOwner owner, string name, TagsUsageProvider usageProvider) : base(owner, name)
	{
		_usageProvider = usageProvider;
	}

	private readonly TagsUsageProvider _usageProvider;
	private readonly List<Tag> _keyPointTags = new();
}