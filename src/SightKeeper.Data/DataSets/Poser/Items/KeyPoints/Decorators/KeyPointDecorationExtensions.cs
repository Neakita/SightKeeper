using SightKeeper.Domain.DataSets.Poser;

namespace SightKeeper.Data.DataSets.Poser.Items.KeyPoints.Decorators;

internal static class KeyPointDecorationExtensions
{
	public static KeyPoint WithTracking(this KeyPoint keyPoint, ChangeListener listener)
	{
		return new TrackableKeyPoint(keyPoint, listener);
	}

	public static KeyPoint WithLocking(this KeyPoint keyPoint, Lock editingLock)
	{
		return new LockingKeyPoint(keyPoint, editingLock);
	}

	public static KeyPoint WithNotifications(this KeyPoint keyPoint)
	{
		return new NotifyingKeyPoint(keyPoint);
	}
}