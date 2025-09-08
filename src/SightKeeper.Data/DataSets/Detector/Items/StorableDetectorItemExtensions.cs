using SightKeeper.Domain.DataSets.Assets.Items;

namespace SightKeeper.Data.DataSets.Detector.Items;

internal static class StorableDetectorItemExtensions
{
	public static DetectorItem WithTracking(this DetectorItem item, ChangeListener listener)
	{
		return new TrackableDetectorItem(item, listener);
	}

	public static DetectorItem WithLocking(this DetectorItem item, Lock editingLock)
	{
		return new LockingDetectorItem(item, editingLock);
	}

	public static DetectorItem WithDomainRules(this DetectorItem item)
	{
		return new DomainDetectorItem(item);
	}

	public static DetectorItem WithNotifications(this DetectorItem item)
	{
		return new NotifyingDetectorItem(item);
	}
}