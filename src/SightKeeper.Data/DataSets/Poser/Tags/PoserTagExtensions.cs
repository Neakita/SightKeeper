using SightKeeper.Data.DataSets.Poser.Tags.Decorators;
using SightKeeper.Domain.DataSets.Poser;

namespace SightKeeper.Data.DataSets.Poser.Tags;

internal static class PoserTagExtensions
{
	public static PoserTag WithTracking(this PoserTag tag, ChangeListener changeListener)
	{
		return new TrackablePoserTag(tag, changeListener);
	}

	public static PoserTag WithLocking(this PoserTag tag, Lock editingLock)
	{
		return new LockingPoserTag(tag, editingLock);
	}

	public static PoserTag WithNotifications(this PoserTag tag)
	{
		return new NotifyingPoserTag(tag);
	}

	public static PoserTag WithEditableUsers(this PoserTag tag)
	{
		return new EditableUsersPoserTag(tag);
	}
}