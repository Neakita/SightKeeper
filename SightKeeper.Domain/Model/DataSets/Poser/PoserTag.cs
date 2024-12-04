using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class PoserTag : Tag, TagsOwner
{
	public IReadOnlyCollection<Tag> KeyPointTags => _keyPointTags.AsReadOnly();
	IReadOnlyCollection<Tag> TagsOwner.Tags => KeyPointTags;

	public Tag CreateKeyPointTag(string name)
	{
		Tag tag = new(this, name);
		_keyPointTags.Add(tag);
		return tag;
	}

	public void DeleteKeyPointTag(Tag tag)
	{
		var isRemoved = _keyPointTags.Remove(tag);
		Guard.IsTrue(isRemoved);
	}

	internal PoserTag(TagsOwner owner, string name) : base(owner, name)
	{
	}


	private readonly List<Tag> _keyPointTags = new();
}