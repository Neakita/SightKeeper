using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Data.DataSets;

public sealed class StorableClassifierDataSetFactory : DataSetFactory<ClassifierDataSet>
{
	public StorableClassifierDataSetFactory(ChangeListener changeListener, Lock editingLock)
	{
		_wrapper = new ClassifierDataSetWrapper();
	}

	public ClassifierDataSet CreateDataSet()
	{
		var inMemorySet = new InMemoryClassifierDataSet(_tagFactory, _assetFactory);
		return _wrapper.Wrap(inMemorySet);
	}

	private readonly ClassifierDataSetWrapper _wrapper;
	private readonly StorableTagFactory _tagFactory = new();
	private readonly StorableClassifierAssetFactory _assetFactory = new();
}