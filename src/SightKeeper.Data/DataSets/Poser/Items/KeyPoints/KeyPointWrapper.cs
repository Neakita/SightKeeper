using SightKeeper.Data.DataSets.Poser.Items.KeyPoints.Decorators;
using SightKeeper.Domain.DataSets.Poser;

namespace SightKeeper.Data.DataSets.Poser.Items.KeyPoints;

internal sealed class KeyPointWrapper(ChangeListener changeListener, Lock editingLock)
{
	public KeyPoint Wrap(KeyPoint keyPoint)
	{
		return keyPoint
			.WithTracking(changeListener)
			.WithLocking(editingLock)
			.WithNotifications();
	}
}