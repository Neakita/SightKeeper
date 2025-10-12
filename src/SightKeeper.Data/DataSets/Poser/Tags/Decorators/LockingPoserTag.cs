using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Poser.Tags.Decorators;

internal sealed class LockingPoserTag(PoserTag inner, Lock editingLock) : PoserTag, Decorator<PoserTag>
{
	public TagsContainer<PoserTag> Owner => inner.Owner;
	public IReadOnlyCollection<TagUser> Users => inner.Users;
	public IReadOnlyList<Tag> KeyPointTags => inner.KeyPointTags;
	public PoserTag Inner => inner;

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

	public void AddUser(TagUser user)
	{
		inner.AddUser(user);
	}

	public void RemoveUser(TagUser user)
	{
		inner.RemoveUser(user);
	}

	public Tag CreateKeyPointTag(string name)
	{
		lock (editingLock)
			return inner.CreateKeyPointTag(name);
	}

	public void DeleteKeyPointTagAt(int index)
	{
		lock (editingLock)
			inner.DeleteKeyPointTagAt(index);
	}

	public void DeleteKeyPointTag(Tag tag)
	{
		lock (editingLock)
			inner.DeleteKeyPointTag(tag);
	}
}