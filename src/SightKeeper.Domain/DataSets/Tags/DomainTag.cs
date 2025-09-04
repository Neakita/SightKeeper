namespace SightKeeper.Domain.DataSets.Tags;

public sealed class DomainTag(Tag inner) : Tag
{
	public string Name
	{
		get => inner.Name;
		set
		{
			TagsConflictingNameException.ThrowIfNameConflicts(Owner.Tags, this, value);
			inner.Name = value;
		}
	}

	public uint Color
	{
		get => inner.Color;
		set => inner.Color = value;
	}

	public TagsContainer<Tag> Owner => inner.Owner;
	public IReadOnlyCollection<TagUser> Users => inner.Users;

	public void AddUser(TagUser user)
	{
		inner.AddUser(user);
	}

	public void RemoveUser(TagUser user)
	{
		inner.RemoveUser(user);
	}
}