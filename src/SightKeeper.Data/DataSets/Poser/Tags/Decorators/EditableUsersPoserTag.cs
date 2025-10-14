using CommunityToolkit.Diagnostics;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Poser.Tags.Decorators;

internal sealed class EditableUsersPoserTag(PoserTag inner) : PoserTag, EditableTagUsers, Decorator<PoserTag>
{
	public TagsContainer<PoserTag> Owner => inner.Owner;
	public IReadOnlyCollection<TagUser> Users => inner.Users;
	public IReadOnlyList<Tag> KeyPointTags => inner.KeyPointTags;
	public PoserTag Inner => inner;

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

	public Tag CreateKeyPointTag(string name)
	{
		return inner.CreateKeyPointTag(name);
	}

	public void DeleteKeyPointTagAt(int index)
	{
		inner.DeleteKeyPointTagAt(index);
	}

	public void DeleteKeyPointTag(Tag tag)
	{
		inner.DeleteKeyPointTag(tag);
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