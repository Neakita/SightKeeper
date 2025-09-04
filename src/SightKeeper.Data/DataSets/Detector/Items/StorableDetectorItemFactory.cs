using SightKeeper.Data.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Detector.Items;

internal sealed class StorableDetectorItemFactory(ChangeListener changeListener, Lock editingLock) : AssetItemFactory<DetectorItem>
{
	public DetectorItem CreateItem(Tag tag)
	{
		return new InMemoryDetectorItem(default, tag)
			.WithTracking(changeListener)
			.WithLocking(editingLock)
			.WithDomainRules()
			.WithNotifications();
	}
}