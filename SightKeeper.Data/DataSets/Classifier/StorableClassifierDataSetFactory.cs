using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Data.DataSets.Classifier;

internal sealed class StorableClassifierDataSetFactory(ClassifierDataSetWrapper wrapper) : DataSetFactory<ClassifierDataSet>
{
	public ClassifierDataSet CreateDataSet()
	{
		StorableTagFactory tagFactory = new();
		StorableClassifierAssetFactory assetFactory = new();
		StorableWeightsWrapper weightsWrapper = new();
		var inMemorySet = new InMemoryClassifierDataSet(tagFactory, assetFactory, weightsWrapper);
		tagFactory.TagsOwner = inMemorySet.TagsLibrary;
		assetFactory.TagsOwner = inMemorySet.TagsLibrary;
		return wrapper.Wrap(inMemorySet);
	}
}