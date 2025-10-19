using SightKeeper.Data.DataSets.Poser.Items.Decorators;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets.Poser;

namespace SightKeeper.Data.DataSets.Poser.Items;

internal static class PoserItemExtensions
{
	public static PoserItem WithTracking(this PoserItem item, ChangeListener listener)
	{
		return new TrackablePoserItem(item, listener);
	}

	public static PoserItem WithLocking(this PoserItem item, Lock editingLock)
	{
		return new LockingPoserItem(item, editingLock);
	}

	public static PoserItem WithDomainRules(this PoserItem item)
	{
		return new DomainPoserItem(item);
	}

	public static PoserItem WithNotifications(this PoserItem item)
	{
		return new NotifyingPoserItem(item);
	}

	public static PoserItem WithObservableKeyPoints(this PoserItem item)
	{
		return new ObservableKeyPointsPoserItem(item);
	}
}