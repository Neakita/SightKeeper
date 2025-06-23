using SightKeeper.Data.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Data;

internal static class Extensions
{
	public static TTarget UnWrapDecorator<TTarget>(this object source)
	{
		if (source is TTarget target)
			return target;
		if (source is Decorator<object> decorator)
			return UnWrapDecorator<TTarget>(decorator.Inner);
		throw new ArgumentException($"Provided object of type {source.GetType()} could not be unwrapped to {typeof(TTarget)}");
	}

	public static ClassifierDataSet WithTracking(this ClassifierDataSet set, ChangeListener listener)
	{
		return new TrackableClassifierDataSet(set, listener);
	}

	public static ClassifierDataSet WithLocking(this ClassifierDataSet set, Lock editingLock)
	{
		return new LockingClassifierDataSet(set, editingLock);
	}

	public static ClassifierDataSet WithDomainRules(this ClassifierDataSet set)
	{
		return new DomainClassifierDataSet(set);
	}
}