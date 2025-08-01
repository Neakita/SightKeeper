namespace SightKeeper.Data.DataSets.Tags;

internal static class TagExtensions
{
	public static StorableTag WithTracking(this StorableTag tag, ChangeListener changeListener)
	{
		return new TrackableTag(tag, changeListener);
	}

	public static StorableTag WithLocking(this StorableTag tag, Lock editingLock)
	{
		return new LockingTag(tag, editingLock);
	}
}