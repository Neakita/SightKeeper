using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Tags;

internal sealed class TrackableTag(StorableTag inner, ChangeListener changeListener) : StorableTag
{
	public string Name
	{
		get => inner.Name;
		set
		{
			inner.Name = value;
			changeListener.SetDataChanged();
		}
	}

	public uint Color
	{
		get => inner.Color;
		set
		{
			inner.Color = value;
			changeListener.SetDataChanged();
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