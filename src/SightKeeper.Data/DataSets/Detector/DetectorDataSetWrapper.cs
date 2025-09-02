namespace SightKeeper.Data.DataSets.Detector;

public sealed class DetectorDataSetWrapper(ChangeListener changeListener, Lock editingLock)
{
	public StorableDetectorDataSet Wrap(StorableDetectorDataSet set)
	{
		return set
			.WithTracking(changeListener)
			.WithLocking(editingLock)
			.WithWeightsDataRemoving()
			.WithObservableLibraries()
			.WithDomainRules();
	}
}