using CommunityToolkit.Diagnostics;
using SightKeeper.Data.DataSets.Assets.Items;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Detector.Items;

internal sealed class StorableDetectorItemFactory(
	ChangeListener changeListener,
	Lock editingLock)
	: AssetItemFactory<DetectorItem>, PostWrappingInitializable<DataSet<Tag, ReadOnlyAsset>>
{
	public void Initialize(DataSet<Tag, ReadOnlyAsset> wrapped)
	{
		_tagsContainer = wrapped.TagsLibrary;
	}

	public DetectorItem CreateItem()
	{
		Guard.IsNotNull(_tagsContainer);
		var tag = _tagsContainer.Tags[0];
		return new InMemoryDetectorItem(default, tag)
			.WithTracking(changeListener)
			.WithLocking(editingLock)
			.WithDomainRules()
			.WithNotifications();
	}

	private TagsContainer<Tag>? _tagsContainer;
}