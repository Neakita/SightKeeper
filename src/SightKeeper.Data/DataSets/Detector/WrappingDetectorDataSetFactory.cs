using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Data.DataSets.Assets.Items;
using SightKeeper.Data.DataSets.Detector.Items;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Detector;

public sealed class WrappingDetectorDataSetFactory(
	DataSetWrapper wrapper,
	ChangeListener changeListener,
	Lock editingLock)
	: DataSetFactory<Tag, ItemsAsset<DetectorItem>>
{
	public DataSet<Tag, ItemsAsset<DetectorItem>> CreateDataSet()
	{
		var tagFactory = new StorableTagFactory(changeListener, editingLock);
		var itemFactory = new StorableDetectorItemFactory(changeListener, editingLock);
		var assetFactory = new StorableItemsAssetFactory<DetectorItem>(itemFactory, changeListener, editingLock);
		var inMemorySet = new InMemoryDataSet<Tag, ItemsAsset<DetectorItem>>(tagFactory, assetFactory, new StorableWeightsWrapper());
		var wrappedSet = wrapper.Wrap(inMemorySet);
		tagFactory.TagsOwner = wrappedSet.TagsLibrary;
		assetFactory.TagsOwner = wrappedSet.TagsLibrary;
		return wrappedSet;
	}
}