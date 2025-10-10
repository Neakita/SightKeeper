using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Data.DataSets.Assets.Items;
using SightKeeper.Data.DataSets.Poser.Items;
using SightKeeper.Data.DataSets.Poser.Items.KeyPoints;
using SightKeeper.Data.DataSets.Poser.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;

namespace SightKeeper.Data.DataSets.Poser;

internal sealed class WrappingPoserDataSetFactory(
	DataSetWrapper<PoserTag, ItemsAsset<PoserItem>> wrapper,
	ChangeListener changeListener,
	Lock editingLock,
	KeyPointFactory keyPointFactory)
	: DataSetFactory<PoserTag, ItemsAsset<PoserItem>>
{
	public DataSet<PoserTag, ItemsAsset<PoserItem>> CreateDataSet()
	{
		var tagFactory = new StorablePoserTagFactory(changeListener, editingLock);
		var itemFactory = new StorablePoserItemFactory(changeListener, editingLock, keyPointFactory);
		var assetFactory = new StorableItemsAssetFactory<PoserItem>(itemFactory, changeListener, editingLock);
		var inMemorySet = new InMemoryDataSet<PoserTag, ItemsAsset<PoserItem>>(tagFactory, assetFactory, new StorableWeightsWrapper());
		var wrappedSet = wrapper.Wrap(inMemorySet);
		itemFactory.TagsContainer = inMemorySet.TagsLibrary;
		tagFactory.TagsOwner = wrappedSet.TagsLibrary;
		assetFactory.TagsOwner = wrappedSet.TagsLibrary;
		return wrappedSet;
	}
}