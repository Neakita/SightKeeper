using SightKeeper.Application;
using SightKeeper.Data.DataSets.Assets.Items;
using SightKeeper.Data.DataSets.Detector.Items;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Detector;

internal sealed class WrappingDetectorDataSetFactory(
	Wrapper<DataSet<Tag, ItemsAsset<DetectorItem>>> wrapper,
	ChangeListener changeListener,
	Lock editingLock)
	: Factory<DataSet<Tag, ItemsAsset<DetectorItem>>>
{
	public DataSet<Tag, ItemsAsset<DetectorItem>> Create()
	{
		var tagFactory = new StorableTagFactory(changeListener, editingLock);
		var itemFactory = new StorableDetectorItemFactory(changeListener, editingLock);
		var assetFactory = new StorableItemsAssetFactory<DetectorItem>(itemFactory, changeListener, editingLock);
		var inMemorySet = new InMemoryDataSet<Tag, ItemsAsset<DetectorItem>>(tagFactory, assetFactory, new StorableWeightsWrapper());
		var wrappedSet = wrapper.Wrap(inMemorySet);
		itemFactory.TagsContainer = inMemorySet.TagsLibrary;
		tagFactory.TagsOwner = wrappedSet.TagsLibrary;
		assetFactory.TagsOwner = wrappedSet.TagsLibrary;
		return wrappedSet;
	}
}