using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Data.Model.DataSets;

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