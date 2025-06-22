using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Data.DataSets.Classifier;

internal sealed class StorableClassifierDataSetFactory : DataSetFactory<ClassifierDataSet>
{
	public StorableClassifierDataSetFactory(ChangeListener changeListener, Lock editingLock)
	{
		_wrapper = new ClassifierDataSetWrapper();
	}

	public ClassifierDataSet CreateDataSet()
	{
		StorableTagFactory tagFactory = new();
		StorableClassifierAssetFactory assetFactory = new();
		var inMemorySet = new InMemoryClassifierDataSet(tagFactory, assetFactory);
		tagFactory.TagsOwner = inMemorySet.TagsLibrary;
		assetFactory.TagsOwner = inMemorySet.TagsLibrary;
		return _wrapper.Wrap(inMemorySet);
	}

	private readonly ClassifierDataSetWrapper _wrapper;
}