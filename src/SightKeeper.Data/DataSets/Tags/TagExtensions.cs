using SightKeeper.Data.DataSets.Tags.Decorators;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Tags;

internal static class TagExtensions
{
	public static Tag WithTracking(this Tag tag, ChangeListener changeListener)
	{
		return new TrackableTag(tag, changeListener);
	}

	public static Tag WithLocking(this Tag tag, Lock editingLock)
	{
		return new LockingTag(tag, editingLock);
	}

	public static Tag WithNotifications(this Tag tag)
	{
		return new NotifyingTag(tag);
	}

	public static Tag WithEditableUsers(this Tag tag)
	{
		return new EditableUsersTag(tag);
	}

	public static Tag WithIndex(this Tag tag)
	{
		return new IndexedTag(tag);
	}
}