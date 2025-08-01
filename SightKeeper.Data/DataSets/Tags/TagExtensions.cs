namespace SightKeeper.Data.DataSets.Tags;

internal static class TagExtensions
{
	public static StorableTag WithTracking(this StorableTag tag, ChangeListener changeListener)
	{
		return new TrackableTag(tag, changeListener);
	}
}