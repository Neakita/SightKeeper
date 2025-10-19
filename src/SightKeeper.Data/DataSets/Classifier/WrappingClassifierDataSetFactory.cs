using SightKeeper.Application;
using SightKeeper.Data.DataSets.Classifier.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Classifier;

internal sealed class WrappingClassifierDataSetFactory(
	DataSetWrapper<Tag, ClassifierAsset> wrapper,
	ChangeListener changeListener,
	Lock editingLock)
	: Factory<DataSet<Tag, ClassifierAsset>>
{
	public DataSet<Tag, ClassifierAsset> Create()
	{
		StorableTagFactory tagFactory = new(changeListener, editingLock);
		StorableClassifierAssetFactory assetFactory = new(changeListener, editingLock);
		StorableWeightsWrapper weightsWrapper = new();
		var inMemorySet = new InMemoryDataSet<Tag, ClassifierAsset>(tagFactory, assetFactory, weightsWrapper);
		var wrappedSet = wrapper.Wrap(inMemorySet);
		tagFactory.TagsOwner = wrappedSet.TagsLibrary;
		assetFactory.TagsOwner = wrappedSet.TagsLibrary;
		return wrappedSet;
	}
}