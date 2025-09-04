using SightKeeper.Domain.DataSets;

namespace SightKeeper.Data.DataSets;

public sealed class DataSetWrapper(ChangeListener changeListener, Lock editingLock)
{
	public DataSet<TAsset> Wrap<TAsset>(DataSet<TAsset> set)
	{
		return set

			// Tracking is locked because we don't want potential double saving when after modifying saving thread will immediately save and consider changes handled,
			// and then tracking decorator will send another notification.
			.WithTracking(changeListener)

			// Locking of domain rules can be relatively computationally heavy,
			// for example when removing images range every image should be checked if it is used by some asset,
			// so locking appears only after domain rules validated.
			.WithLocking(editingLock)

			// Weights data removing could be expansive (we can remove large weights files),
			// and there is no need in lock because lock should affect AppData only,
			// not the weights files,
			// so it shouldn't be locked
			.WithWeightsDataRemoving()

			// We don't want to add or remove users when changes are invalid by domain rules,
			// so it should be behind domain rules.
			// Events better be fired after tag users updated, so put it behind ObservableLibraries.
			.WithTagUsersTracking()

			// Changes shouldn't be observed if they aren't valid,
			// so it should be behind domain rules
			.WithObservableLibraries()

			// If domain rule is violated and throws an exception,
			// it should fail as fast as possible and have smaller stack strace
			.WithDomainRules()

			// INPC interface should be exposed to consumer,
			// so he can type test and cast it,
			// so it should be the outermost layer
			.WithNotifications();
	}
}