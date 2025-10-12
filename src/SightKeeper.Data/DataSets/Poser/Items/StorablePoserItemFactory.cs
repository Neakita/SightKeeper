using CommunityToolkit.Diagnostics;
using SightKeeper.Data.DataSets.Assets.Items;
using SightKeeper.Data.DataSets.Poser.Items.KeyPoints;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Poser.Items;

internal sealed class StorablePoserItemFactory(ChangeListener changeListener, Lock editingLock, KeyPointFactory keyPointFactory) : AssetItemFactory<PoserItem>
{
	public TagsContainer<PoserTag>? TagsContainer { get; set; }

	public PoserItem CreateItem()
	{
		Guard.IsNotNull(TagsContainer);
		var tag = TagsContainer.Tags[0];
		return new InMemoryPoserItem(tag, keyPointFactory)
			.WithTracking(changeListener)
			.WithLocking(editingLock)
			.WithObservableKeyPoints()
			.WithDomainRules()
			.WithNotifications();
	}
}