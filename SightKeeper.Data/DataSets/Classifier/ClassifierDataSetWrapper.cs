using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Data.DataSets.Classifier;

internal sealed class ClassifierDataSetWrapper
{
	public ClassifierDataSet Wrap(InMemoryClassifierDataSet set)
	{
		return set;
		
			/*.WithTracking(changeListener)
			.WithLocking(editingLock)
			.WithDomainRules()*/
	}
}