using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Tags;

internal sealed class LockingTag(StorableTag inner, Lock editingLock): StorableTag
{
	public string Name
	{
		get => inner.Name;
		set
		{
			lock (editingLock)
				inner.Name = value;
		}
	}

	public uint Color
	{
		get => inner.Color;
		set
		{
			lock (editingLock)
				inner.Color = value;
		}
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