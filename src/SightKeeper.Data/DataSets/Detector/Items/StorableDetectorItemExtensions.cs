using SightKeeper.Domain.DataSets.Detector;

namespace SightKeeper.Data.DataSets.Detector.Items;

internal static class StorableDetectorItemExtensions
{
	public static StorableDetectorItem WithTracking(this StorableDetectorItem item, ChangeListener listener)
	{
		return new TrackableDetectorItem(item, listener);
	}

	public static StorableDetectorItem WithLocking(this StorableDetectorItem item, Lock editingLock)
	{
		return new LockingDetectorItem(item, editingLock);
	}

	public static StorableDetectorItem WithDomainRules(this StorableDetectorItem item)
	{
		return new StorableDetectorItemExtension(new DomainDetectorItem(item), item);
	}

	public static StorableDetectorItem WithNotifications(this StorableDetectorItem item)
	{
		return new NotifyingDetectorItem(item);
	}
}