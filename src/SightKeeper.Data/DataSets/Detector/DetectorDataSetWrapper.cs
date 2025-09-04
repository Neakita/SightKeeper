using SightKeeper.Domain.DataSets.Detector;

namespace SightKeeper.Data.DataSets.Detector;

public sealed class DetectorDataSetWrapper(ChangeListener changeListener, Lock editingLock)
{
	public DetectorDataSet Wrap(DetectorDataSet set)
	{
		return set
			.WithTracking(changeListener)
			.WithLocking(editingLock)
			.WithWeightsDataRemoving()
			.WithObservableLibraries()
			.WithDomainRules();
	}
}