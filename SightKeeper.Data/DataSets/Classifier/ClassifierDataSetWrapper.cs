using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Data.DataSets.Classifier;

internal sealed class ClassifierDataSetWrapper(ChangeListener changeListener, Lock editingLock)
{
	public ClassifierDataSet Wrap(InMemoryClassifierDataSet set)
	{
		return set
			.WithTracking(changeListener)
			.WithLocking(editingLock)
			.WithDomainRules();
	}
}