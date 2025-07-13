namespace SightKeeper.Data.DataSets.Classifier;

internal sealed class ClassifierDataSetWrapper(ChangeListener changeListener, Lock editingLock)
{
	public StorableClassifierDataSet Wrap(StorableClassifierDataSet set)
	{
		return set
			.WithTracking(changeListener)
			.WithLocking(editingLock)
			.WithDomainRules();
	}
}