using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Data.DataSets.Classifier;

internal static class ClassifierDataSetExtensions
{
	public static StorableClassifierDataSet WithTracking(this StorableClassifierDataSet set, ChangeListener listener)
	{
		return new TrackableClassifierDataSet(set, listener);
	}

	public static StorableClassifierDataSet WithLocking(this StorableClassifierDataSet set, Lock editingLock)
	{
		return new LockingClassifierDataSet(set, editingLock);
	}

	public static StorableClassifierDataSet WithDomainRules(this StorableClassifierDataSet set)
	{
		return new StorableClassifierDataSetExtension(new DomainClassifierDataSet(set), set);
	}
}