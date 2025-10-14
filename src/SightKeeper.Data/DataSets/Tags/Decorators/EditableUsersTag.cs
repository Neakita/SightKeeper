using CommunityToolkit.Diagnostics;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Tags.Decorators;

internal sealed class EditableUsersTag(Tag inner) : Tag, EditableTagUsers, Decorator<Tag>
{
	public IReadOnlyCollection<TagUser> Users => _users;
	public TagsContainer<Tag> Owner => inner.Owner;
	public Tag Inner => inner;

	public string Name
	{
		get => inner.Name;
		set => inner.Name = value;
	}

	public uint Color
	{
		get => inner.Color;
		set => inner.Color = value;
	}

	public void AddUser(TagUser user)
	{
		_users.Add(user);
	}

	public void RemoveUser(TagUser user)
	{
		bool isRemoved = _users.Remove(user);
		Guard.IsTrue(isRemoved);
	}

	private readonly HashSet<TagUser> _users = new();
}