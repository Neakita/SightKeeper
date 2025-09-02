using SightKeeper.Data.DataSets.Assets.Items;
using SightKeeper.Data.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Detector.Items;

internal sealed class StorableDetectorItemFactory(ChangeListener changeListener, Lock editingLock) : AssetItemFactory<StorableDetectorItem>
{
	public StorableDetectorItem CreateItem(StorableTag tag)
	{
		return new InMemoryDetectorItem(default, tag)
			.WithTracking(changeListener)
			.WithLocking(editingLock)
			.WithDomainRules()
			.WithNotifications();
	}
}