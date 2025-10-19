using CommunityToolkit.Diagnostics;
using SightKeeper.Data.DataSets.Assets.Items;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Detector.Items;

internal sealed class StorableDetectorItemFactory(ChangeListener changeListener, Lock editingLock) : AssetItemFactory<DetectorItem>
{
	public TagsContainer<Tag>? TagsContainer { get; set; }

	public DetectorItem CreateItem()
	{
		Guard.IsNotNull(TagsContainer);
		var tag = TagsContainer.Tags[0];
		return new InMemoryDetectorItem(default, tag)
			.WithTracking(changeListener)
			.WithLocking(editingLock)
			.WithDomainRules()
			.WithNotifications();
	}
}