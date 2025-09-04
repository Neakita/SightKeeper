using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Data.DataSets.Classifier.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Data.DataSets.Classifier;

public sealed class WrappingClassifierDataSetFactory(DataSetWrapper wrapper, ChangeListener changeListener, Lock editingLock)
	: DataSetFactory<ClassifierAsset>
{
	public DataSet<ClassifierAsset> CreateDataSet()
	{
		StorableTagFactory tagFactory = new(changeListener, editingLock);
		StorableClassifierAssetFactory assetFactory = new(changeListener, editingLock);
		StorableWeightsWrapper weightsWrapper = new();
		var inMemorySet = new InMemoryDataSet<ClassifierAsset>(tagFactory, assetFactory, weightsWrapper);
		var wrappedSet = wrapper.Wrap(inMemorySet);
		tagFactory.TagsOwner = wrappedSet.TagsLibrary;
		assetFactory.TagsOwner = wrappedSet.TagsLibrary;
		return wrappedSet;
	}
}