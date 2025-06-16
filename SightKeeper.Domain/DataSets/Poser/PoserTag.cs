using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser;

public sealed class PoserTag : DomainTag, TagsOwner<DomainTag>
{
	public IReadOnlyList<DomainTag> KeyPointTags => _keyPointTags.AsReadOnly();
	IReadOnlyList<DomainTag> TagsContainer<DomainTag>.Tags => KeyPointTags;

	public DomainTag CreateKeyPointTag(string name)
	{
		DomainTag tag = new(this, name);
		_keyPointTags.Add(tag);
		return tag;
	}

	public void DeleteKeyPointTagAt(int index)
	{
		var tag = _keyPointTags[index];
		TagIsInUseException.ThrowForDeletionIfInUse(tag);
		_keyPointTags.RemoveAt(index);
	}

	public void DeleteKeyPointTag(DomainTag tag)
	{
		TagIsInUseException.ThrowForDeletionIfInUse(tag);
		var isRemoved = _keyPointTags.Remove(tag);
		if (!isRemoved)
			throw new ArgumentException("Specified tag was not found and therefore not deleted", nameof(tag));
	}

	DomainTag TagsOwner<DomainTag>.CreateTag(string name)
	{
		return CreateKeyPointTag(name);
	}

	void TagsOwner<DomainTag>.DeleteTagAt(int index)
	{
		DeleteKeyPointTagAt(index);
	}

	internal PoserTag(TagsContainer<DomainTag> owner, string name) : base(owner, name)
	{
	}

	private readonly List<DomainTag> _keyPointTags = new();
}