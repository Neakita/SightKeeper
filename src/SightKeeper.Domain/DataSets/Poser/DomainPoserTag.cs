using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser;

public sealed class DomainPoserTag(PoserTag inner) : PoserTag, Decorator<PoserTag>
{
	public string Name
	{
		get => inner.Name;
		set
		{
			TagsConflictingNameException.ThrowIfNameConflicts(inner.Owner.Tags, this, value);
			inner.Name = value;
		}
	}

	public uint Color
	{
		get => inner.Color;
		set => inner.Color = value;
	}

	public TagsContainer<PoserTag> Owner => inner.Owner;
	public IReadOnlyCollection<TagUser> Users => inner.Users;
	public IReadOnlyList<Tag> KeyPointTags => inner.KeyPointTags;
	public PoserTag Inner => inner;

	public Tag CreateKeyPointTag(string name)
	{
		return inner.CreateKeyPointTag(name);
	}

	public void DeleteKeyPointTagAt(int index)
	{
		var tag = KeyPointTags[index];
		TagIsInUseException.ThrowForDeletionIfInUse(tag);
		inner.DeleteKeyPointTagAt(index);
	}

	public void DeleteKeyPointTag(Tag tag)
	{
		TagIsInUseException.ThrowForDeletionIfInUse(tag);
		inner.DeleteKeyPointTag(tag);
	}

	public void AddUser(TagUser user)
	{
		inner.AddUser(user);
	}

	public void RemoveUser(TagUser user)
	{
		inner.RemoveUser(user);
	}
}